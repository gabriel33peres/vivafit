using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Models;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia as avaliações físicas dos alunos.</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>Inicializa o controller de avaliações.</summary>
        public AvaliacoesController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar avaliação
        /// <summary>Cadastra uma nova avaliação física.</summary>
        /// <param name="avaliacao">Dados da avaliação física.</param>
        [HttpPost]
        public async Task<ActionResult<Avaliacao>> CadastrarAvaliacao(Avaliacao avaliacao)
        {
            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarAvaliacao),
                new { id = avaliacao.Id },
                avaliacao
            );
        }

        // Listar todas as avaliações
        /// <summary>Lista todas as avaliações físicas.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> ListarAvaliacoes()
        {
            var avaliacoes = await _context.Avaliacoes.ToListAsync();

            return Ok(avaliacoes);
        }

        // Consultar avaliação por ID
        /// <summary>Consulta uma avaliação pelo identificador.</summary>
        /// <param name="id">Identificador da avaliação.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Avaliacao>> ConsultarAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);

            if (avaliacao == null)
            {
                return NotFound("Avaliação não encontrada.");
            }

            return Ok(avaliacao);
        }

        // Alterar avaliação
        /// <summary>Atualiza uma avaliação física.</summary>
        /// <param name="id">Identificador da avaliação.</param>
        /// <param name="avaliacao">Novos dados da avaliação.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarAvaliacao(
            int id,
            Avaliacao avaliacao)
        {
            if (id != avaliacao.Id)
            {
                return BadRequest(
                    "O ID informado não corresponde à avaliação."
                );
            }

            var avaliacaoExistente =
                await _context.Avaliacoes.FindAsync(id);

            if (avaliacaoExistente == null)
            {
                return NotFound("Avaliação não encontrada.");
            }

            avaliacaoExistente.AlunoId = avaliacao.AlunoId;
            avaliacaoExistente.ProfessorId = avaliacao.ProfessorId;
            avaliacaoExistente.DataAvaliacao = avaliacao.DataAvaliacao;
            avaliacaoExistente.Peso = avaliacao.Peso;
            avaliacaoExistente.Altura = avaliacao.Altura;
            avaliacaoExistente.PercentualGordura = avaliacao.PercentualGordura;
            avaliacaoExistente.MassaMuscular = avaliacao.MassaMuscular;
            avaliacaoExistente.Observacoes = avaliacao.Observacoes;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir avaliação
        /// <summary>Exclui uma avaliação pelo identificador.</summary>
        /// <param name="id">Identificador da avaliação.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirAvaliacao(int id)
        {
            var avaliacao =
                await _context.Avaliacoes.FindAsync(id);

            if (avaliacao == null)
            {
                return NotFound("Avaliação não encontrada.");
            }

            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}