using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Models;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia os pagamentos das matrículas.</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>Inicializa o controller de pagamentos.</summary>
        public PagamentosController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar pagamento
        /// <summary>Cadastra um novo pagamento.</summary>
        /// <param name="pagamento">Dados do pagamento.</param>
        [HttpPost]
        public async Task<ActionResult<Pagamento>> CadastrarPagamento(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarPagamento),
                new { id = pagamento.Id },
                pagamento
            );
        }

        // Listar todos os pagamentos
        /// <summary>Lista todos os pagamentos.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagamento>>> ListarPagamentos()
        {
            var pagamentos = await _context.Pagamentos.ToListAsync();

            return Ok(pagamentos);
        }

        // Consultar pagamento por ID
        /// <summary>Consulta um pagamento pelo identificador.</summary>
        /// <param name="id">Identificador do pagamento.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> ConsultarPagamento(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);

            if (pagamento == null)
            {
                return NotFound("Pagamento não encontrado.");
            }

            return Ok(pagamento);
        }

        // Alterar pagamento
        /// <summary>Atualiza um pagamento.</summary>
        /// <param name="id">Identificador do pagamento.</param>
        /// <param name="pagamento">Novos dados do pagamento.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarPagamento(
            int id,
            Pagamento pagamento)
        {
            if (id != pagamento.Id)
            {
                return BadRequest(
                    "O ID informado não corresponde ao pagamento."
                );
            }

            var pagamentoExistente =
                await _context.Pagamentos.FindAsync(id);

            if (pagamentoExistente == null)
            {
                return NotFound("Pagamento não encontrado.");
            }

            pagamentoExistente.MatriculaId = pagamento.MatriculaId;
            pagamentoExistente.Valor = pagamento.Valor;
            pagamentoExistente.DataVencimento = pagamento.DataVencimento;
            pagamentoExistente.DataPagamento = pagamento.DataPagamento;
            pagamentoExistente.Status = pagamento.Status;
            pagamentoExistente.FormaPagamento = pagamento.FormaPagamento;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir pagamento
        /// <summary>Exclui um pagamento.</summary>
        /// <param name="id">Identificador do pagamento.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirPagamento(int id)
        {
            var pagamento =
                await _context.Pagamentos.FindAsync(id);

            if (pagamento == null)
            {
                return NotFound("Pagamento não encontrado.");
            }

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}