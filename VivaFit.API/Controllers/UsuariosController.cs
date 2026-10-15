using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VivaFit.API.Data;
using VivaFit.API.Dtos;
using VivaFit.API.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VivaFit.API.Controllers
{
    /// <summary>Gerencia os usuários e o acesso à API.</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>Inicializa o controller de usuários.</summary>
        public UsuariosController(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Cadastrar usuário
        /// <summary>Cadastra um novo usuário.</summary>
        /// <param name="usuario">Dados do usuário.</param>
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> CadastrarUsuario(
            Usuario usuario)
        {
            var usuarioExistente = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (usuarioExistente != null)
        {
            return Conflict("Já existe um usuário cadastrado com este e-mail.");
        }
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarUsuario),
                new { id = usuario.Id },
                new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Perfil = usuario.Perfil,
                    Ativo = usuario.Ativo,
                    DataCadastro = usuario.DataCadastro
                }
            );
        }

        // Listar usuários
        /// <summary>Lista todos os usuários.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(usuario => new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Perfil = usuario.Perfil,
                    Ativo = usuario.Ativo,
                    DataCadastro = usuario.DataCadastro
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // Consultar usuário por ID
        /// <summary>Consulta um usuário pelo identificador.</summary>
        /// <param name="id">Identificador do usuário.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> ConsultarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Ativo = usuario.Ativo,
                DataCadastro = usuario.DataCadastro
            });
        }

        // Alterar usuário
        /// <summary>Atualiza um usuário.</summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <param name="usuario">Novos dados do usuário.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarUsuario(
            int id,
            Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest(
                    "O ID informado não corresponde ao usuário."
                );
            }

            var usuarioExistente =
                await _context.Usuarios.FindAsync(id);

            if (usuarioExistente == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Senha = usuario.Senha;
            usuarioExistente.Perfil = usuario.Perfil;
            usuarioExistente.Ativo = usuario.Ativo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir usuário
        /// <summary>Autentica um usuário e gera um token JWT.</summary>
        /// <param name="email">E-mail de acesso.</param>
        /// <param name="senha">Senha de acesso.</param>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            string email,
            string senha)
        {
            var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
        {
            return Unauthorized(
            "E-mail ou senha inválidos."
        );
        }

            if (!usuario.Ativo)
        {
             return Unauthorized(
            "Usuário inativo."
            );
        }

            bool senhaValida = BCrypt.Net.BCrypt.Verify(
            senha,
            usuario.Senha
            );

            if (!senhaValida)
        {
             return Unauthorized(
                "E-mail ou senha inválidos."
             );
        }

            var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
              usuario.Id.ToString()
            ),

            new Claim(
            ClaimTypes.Name,
            usuario.Nome
        ),

            new Claim(
            ClaimTypes.Email,
            usuario.Email
        ),

            new Claim(
            ClaimTypes.Role,
            usuario.Perfil
        )
        };

            var chave = _configuration["Jwt:Key"];

            var chaveSegura = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(chave!)
        );

            var credenciais = new SigningCredentials(
                chaveSegura,
             SecurityAlgorithms.HmacSha256
        );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credenciais
        );

            var tokenGerado =
         new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
        {
                mensagem = "Login realizado com sucesso.",
                token = tokenGerado,
                usuario = new
            {
                id = usuario.Id,
                nome = usuario.Nome,
                email = usuario.Email,
                perfil = usuario.Perfil
        }
        });
}
    }
}