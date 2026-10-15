using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Models;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia os registros de frequência dos alunos.</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FrequenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>Inicializa o controller de frequências.</summary>
        public FrequenciasController(AppDbContext context)
        {
            _context = context;
        }

        // Registrar entrada do aluno
        /// <summary>Registra a entrada ou saída de um aluno.</summary>
        /// <param name="frequencia">Dados do registro de frequência.</param>
        [HttpPost]
        public async Task<ActionResult<Frequencia>> RegistrarEntrada(
            Frequencia frequencia)
        {
            _context.Frequencias.Add(frequencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarFrequencia),
                new { id = frequencia.Id },
                frequencia
            );
        }

        // Listar todas as frequências
        /// <summary>Lista todos os registros de frequência.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Frequencia>>> ListarFrequencias()
        {
            var frequencias = await _context.Frequencias.ToListAsync();

            return Ok(frequencias);
        }

        // Consultar frequência por ID
        /// <summary>Consulta um registro de frequência pelo identificador.</summary>
        /// <param name="id">Identificador do registro.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Frequencia>> ConsultarFrequencia(int id)
        {
            var frequencia = await _context.Frequencias.FindAsync(id);

            if (frequencia == null)
            {
                return NotFound("Frequência não encontrada.");
            }

            return Ok(frequencia);
        }

        // Alterar frequência
        /// <summary>Atualiza um registro de frequência.</summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="frequencia">Novos dados do registro.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarFrequencia(
            int id,
            Frequencia frequencia)
        {
            if (id != frequencia.Id)
            {
                return BadRequest(
                    "O ID informado não corresponde à frequência."
                );
            }

            var frequenciaExistente =
                await _context.Frequencias.FindAsync(id);

            if (frequenciaExistente == null)
            {
                return NotFound("Frequência não encontrada.");
            }

            frequenciaExistente.AlunoId = frequencia.AlunoId;
            frequenciaExistente.DataEntrada = frequencia.DataEntrada;
            frequenciaExistente.DataSaida = frequencia.DataSaida;
            frequenciaExistente.Tipo = frequencia.Tipo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir frequência
        /// <summary>Exclui um registro de frequência.</summary>
        /// <param name="id">Identificador do registro.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirFrequencia(int id)
        {
            var frequencia = await _context.Frequencias.FindAsync(id);

            if (frequencia == null)
            {
                return NotFound("Frequência não encontrada.");
            }

            _context.Frequencias.Remove(frequencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}