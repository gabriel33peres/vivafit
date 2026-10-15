using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Models;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia os alunos cadastrados na academia.</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Adiciona a necessidade de autenticação para acessar os endpoints
    public class AlunosController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>Inicializa o controller de alunos.</summary>
        public AlunosController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar aluno
        /// <summary>Cadastra um novo aluno.</summary>
        /// <param name="aluno">Dados do aluno a ser cadastrado.</param>
        [HttpPost]
        public async Task<ActionResult<Aluno>> CadastrarAluno(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarAluno),
                new { id = aluno.Id },
                aluno
            );
        }

        // Listar todos os alunos
        /// <summary>Lista todos os alunos.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aluno>>> ListarAlunos()
        {
            var alunos = await _context.Alunos.ToListAsync();

            return Ok(alunos);
        }

        // Consultar aluno por ID
        /// <summary>Consulta um aluno pelo identificador.</summary>
        /// <param name="id">Identificador do aluno.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Aluno>> ConsultarAluno(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return NotFound("Aluno não encontrado.");
            }

            return Ok(aluno);
        }

        // Alterar aluno
        /// <summary>Atualiza os dados de um aluno.</summary>
        /// <param name="id">Identificador do aluno.</param>
        /// <param name="aluno">Novos dados do aluno.</param>
        [HttpPut("{id}")]
public async Task<IActionResult> AlterarAluno(int id, Aluno aluno)
{
    if (id != aluno.Id)
    {
        return BadRequest("O ID informado não corresponde ao aluno.");
    }

    var alunoExistente = await _context.Alunos.FindAsync(id);

    if (alunoExistente == null)
    {
        return NotFound("Aluno não encontrado.");
    }

    alunoExistente.Nome = aluno.Nome;
    alunoExistente.CPF = aluno.CPF;
    alunoExistente.Email = aluno.Email;
    alunoExistente.Telefone = aluno.Telefone;
    alunoExistente.DataNascimento = aluno.DataNascimento;
    alunoExistente.Ativo = aluno.Ativo;

    await _context.SaveChangesAsync();

    return NoContent();
}

// Excluir aluno
/// <summary>Exclui um aluno pelo identificador.</summary>
/// <param name="id">Identificador do aluno.</param>
[HttpDelete("{id}")]
public async Task<IActionResult> ExcluirAluno(int id)
{
    var aluno = await _context.Alunos.FindAsync(id);

    if (aluno == null)
    {
        return NotFound("Aluno não encontrado.");
    }

    _context.Alunos.Remove(aluno);
    await _context.SaveChangesAsync();

    return NoContent();
}
    }
}