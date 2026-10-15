namespace VivaFit.API.Models
{
    public class Treino
    {
        public int Id { get; set; }

        public int AlunoId { get; set; }

        public int ProfessorId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Objetivo { get; set; } = string.Empty;

        public string Exercicios { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;
    }
}