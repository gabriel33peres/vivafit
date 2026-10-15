using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Models;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia as matrículas dos alunos.</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculasController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>Inicializa o controller de matrículas.</summary>
        public MatriculasController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar matrícula
        /// <summary>Cadastra uma nova matrícula.</summary>
        /// <param name="matricula">Dados da matrícula.</param>
        [HttpPost]
        public async Task<ActionResult<Matricula>> CadastrarMatricula(Matricula matricula)
        {
            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarMatricula),
                new { id = matricula.Id },
                matricula
            );
        }

        // Listar todas as matrículas
        /// <summary>Lista todas as matrículas.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Matricula>>> ListarMatriculas()
        {
            var matriculas = await _context.Matriculas.ToListAsync();

            return Ok(matriculas);
        }

        // Consultar matrícula por ID
        /// <summary>Consulta uma matrícula pelo identificador.</summary>
        /// <param name="id">Identificador da matrícula.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Matricula>> ConsultarMatricula(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);

            if (matricula == null)
            {
                return NotFound("Matrícula não encontrada.");
            }

            return Ok(matricula);
        }

        // Alterar matrícula
        /// <summary>Atualiza uma matrícula.</summary>
        /// <param name="id">Identificador da matrícula.</param>
        /// <param name="matricula">Novos dados da matrícula.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarMatricula(
            int id,
            Matricula matricula)
        {
            if (id != matricula.Id)
            {
                return BadRequest(
                    "O ID informado não corresponde à matrícula."
                );
            }

            var matriculaExistente =
                await _context.Matriculas.FindAsync(id);

            if (matriculaExistente == null)
            {
                return NotFound("Matrícula não encontrada.");
            }

            matriculaExistente.AlunoId = matricula.AlunoId;
            matriculaExistente.PlanoId = matricula.PlanoId;
            matriculaExistente.DataInicio = matricula.DataInicio;
            matriculaExistente.DataFim = matricula.DataFim;
            matriculaExistente.Status = matricula.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir matrícula
        /// <summary>Exclui uma matrícula.</summary>
        /// <param name="id">Identificador da matrícula.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirMatricula(int id)
        {
            var matricula =
                await _context.Matriculas.FindAsync(id);

            if (matricula == null)
            {
                return NotFound("Matrícula não encontrada.");
            }

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}