using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Models;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia os planos oferecidos pela academia.</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlanosController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>Inicializa o controller de planos.</summary>
        public PlanosController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar plano
        /// <summary>Cadastra um novo plano.</summary>
        /// <param name="plano">Dados do plano.</param>
        [HttpPost]
        public async Task<ActionResult<Plano>> CadastrarPlano(Plano plano)
        {
            _context.Planos.Add(plano);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarPlano),
                new { id = plano.Id },
                plano
            );
        }

        // Listar todos os planos
        /// <summary>Lista todos os planos.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plano>>> ListarPlanos()
        {
            var planos = await _context.Planos.ToListAsync();

            return Ok(planos);
        }

        // Consultar plano por ID
        /// <summary>Consulta um plano pelo identificador.</summary>
        /// <param name="id">Identificador do plano.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Plano>> ConsultarPlano(int id)
        {
            var plano = await _context.Planos.FindAsync(id);

            if (plano == null)
            {
                return NotFound("Plano não encontrado.");
            }

            return Ok(plano);
        }

        // Alterar plano
        /// <summary>Atualiza um plano.</summary>
        /// <param name="id">Identificador do plano.</param>
        /// <param name="plano">Novos dados do plano.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarPlano(int id, Plano plano)
        {
            if (id != plano.Id)
            {
                return BadRequest("O ID informado não corresponde ao plano.");
            }

            var planoExistente = await _context.Planos.FindAsync(id);

            if (planoExistente == null)
            {
                return NotFound("Plano não encontrado.");
            }

            planoExistente.Nome = plano.Nome;
            planoExistente.Descricao = plano.Descricao;
            planoExistente.Valor = plano.Valor;
            planoExistente.DuracaoMeses = plano.DuracaoMeses;
            planoExistente.Ativo = plano.Ativo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir plano
        /// <summary>Exclui um plano.</summary>
        /// <param name="id">Identificador do plano.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirPlano(int id)
        {
            var plano = await _context.Planos.FindAsync(id);

            if (plano == null)
            {
                return NotFound("Plano não encontrado.");
            }

            _context.Planos.Remove(plano);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}