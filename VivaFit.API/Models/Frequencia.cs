namespace VivaFit.API.Models
{
    public class Frequencia
    {
        public int Id { get; set; }

        public int AlunoId { get; set; }

        public DateTime DataEntrada { get; set; } = DateTime.Now;

        public DateTime? DataSaida { get; set; }

        public string Tipo { get; set; } = "Check-in";
    }
}