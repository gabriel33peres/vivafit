namespace VivaFit.API.Models
{
    public class Professor
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;

        public string CREF { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string Especialidade { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}