namespace VivaFit.API.Models
{
    public class Pagamento
    {
        public int Id { get; set; }

        public int MatriculaId { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataVencimento { get; set; }

        public DateTime? DataPagamento { get; set; }

        public string Status { get; set; } = "Pendente";

        public string FormaPagamento { get; set; } = string.Empty;
    }
}