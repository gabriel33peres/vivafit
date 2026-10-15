using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Models;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia os treinos dos alunos.</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TreinosController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>Inicializa o controller de treinos.</summary>
        public TreinosController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar treino
        /// <summary>Cadastra um novo treino.</summary>
        /// <param name="treino">Dados do treino.</param>
        [HttpPost]
        public async Task<ActionResult<Treino>> CadastrarTreino(Treino treino)
        {
            _context.Treinos.Add(treino);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarTreino),
                new { id = treino.Id },
                treino
            );
        }

        // Listar todos os treinos
        /// <summary>Lista todos os treinos.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Treino>>> ListarTreinos()
        {
            var treinos = await _context.Treinos.ToListAsync();

            return Ok(treinos);
        }

        // Consultar treino por ID
        /// <summary>Consulta um treino pelo identificador.</summary>
        /// <param name="id">Identificador do treino.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Treino>> ConsultarTreino(int id)
        {
            var treino = await _context.Treinos.FindAsync(id);

            if (treino == null)
            {
                return NotFound("Treino não encontrado.");
            }

            return Ok(treino);
        }

        // Alterar treino
        /// <summary>Atualiza um treino.</summary>
        /// <param name="id">Identificador do treino.</param>
        /// <param name="treino">Novos dados do treino.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarTreino(
            int id,
            Treino treino)
        {
            if (id != treino.Id)
            {
                return BadRequest(
                    "O ID informado não corresponde ao treino."
                );
            }

            var treinoExistente =
                await _context.Treinos.FindAsync(id);

            if (treinoExistente == null)
            {
                return NotFound("Treino não encontrado.");
            }

            treinoExistente.AlunoId = treino.AlunoId;
            treinoExistente.ProfessorId = treino.ProfessorId;
            treinoExistente.Nome = treino.Nome;
            treinoExistente.Objetivo = treino.Objetivo;
            treinoExistente.Exercicios = treino.Exercicios;
            treinoExistente.Ativo = treino.Ativo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir treino
        /// <summary>Exclui um treino.</summary>
        /// <param name="id">Identificador do treino.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirTreino(int id)
        {
            var treino =
                await _context.Treinos.FindAsync(id);

            if (treino == null)
            {
                return NotFound("Treino não encontrado.");
            }

            _context.Treinos.Remove(treino);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}