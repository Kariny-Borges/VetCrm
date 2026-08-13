namespace VetCrm.Models
{
    public class Servico : EntidadeBase
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}
