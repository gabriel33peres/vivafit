using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Models;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia os professores da academia.</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>Inicializa o controller de professores.</summary>
        public ProfessoresController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar professor
        /// <summary>Cadastra um novo professor.</summary>
        /// <param name="professor">Dados do professor.</param>
        [HttpPost]
        public async Task<ActionResult<Professor>> CadastrarProfessor(Professor professor)
        {
            _context.Professores.Add(professor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarProfessor),
                new { id = professor.Id },
                professor
            );
        }

        // Listar todos os professores
        /// <summary>Lista todos os professores.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professor>>> ListarProfessores()
        {
            var professores = await _context.Professores.ToListAsync();

            return Ok(professores);
        }

        // Consultar professor por ID
        /// <summary>Consulta um professor pelo identificador.</summary>
        /// <param name="id">Identificador do professor.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Professor>> ConsultarProfessor(int id)
        {
            var professor = await _context.Professores.FindAsync(id);

            if (professor == null)
            {
                return NotFound("Professor não encontrado.");
            }

            return Ok(professor);
        }

        // Alterar professor
        /// <summary>Atualiza um professor.</summary>
        /// <param name="id">Identificador do professor.</param>
        /// <param name="professor">Novos dados do professor.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarProfessor(
            int id,
            Professor professor)
        {
            if (id != professor.Id)
            {
                return BadRequest(
                    "O ID informado não corresponde ao professor."
                );
            }

            var professorExistente =
                await _context.Professores.FindAsync(id);

            if (professorExistente == null)
            {
                return NotFound("Professor não encontrado.");
            }

            professorExistente.Nome = professor.Nome;
            professorExistente.CPF = professor.CPF;
            professorExistente.CREF = professor.CREF;
            professorExistente.Email = professor.Email;
            professorExistente.Telefone = professor.Telefone;
            professorExistente.Especialidade = professor.Especialidade;
            professorExistente.Ativo = professor.Ativo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir professor
        /// <summary>Exclui um professor.</summary>
        /// <param name="id">Identificador do professor.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirProfessor(int id)
        {
            var professor =
                await _context.Professores.FindAsync(id);

            if (professor == null)
            {
                return NotFound("Professor não encontrado.");
            }

            _context.Professores.Remove(professor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}