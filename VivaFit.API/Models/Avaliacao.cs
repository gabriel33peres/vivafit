namespace VivaFit.API.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }

        public int AlunoId { get; set; }

        public int ProfessorId { get; set; }

        public DateTime DataAvaliacao { get; set; } = DateTime.Now;

        public decimal Peso { get; set; }

        public decimal Altura { get; set; }

        public decimal PercentualGordura { get; set; }

        public decimal MassaMuscular { get; set; }

        public string Observacoes { get; set; } = string.Empty;
    }
}