namespace VetCrm.Models
{
    public class Produto : EntidadeBase
    {
        public string Nome { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
