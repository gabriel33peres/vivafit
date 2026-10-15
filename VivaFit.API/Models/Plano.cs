namespace VivaFit.API.Models
{
    public class Plano
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public int DuracaoMeses { get; set; }

        public bool Ativo { get; set; } = true;
    }
}