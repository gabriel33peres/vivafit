namespace VivaFit.API.Dtos
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Perfil { get; set; } = string.Empty;

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
