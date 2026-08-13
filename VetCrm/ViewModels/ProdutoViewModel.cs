namespace VetCrm.ViewModels
{
    public class ProdutoViewModel : ViewModelEntidadeBase
    {
        public string Nome { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public CategoriaViewModel? Categoria { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}