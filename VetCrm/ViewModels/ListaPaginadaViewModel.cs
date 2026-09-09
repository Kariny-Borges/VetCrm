namespace VetCrm.ViewModels
{
    public class PaginacaoViewModel
    {
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
        public string Busca { get; set; } = string.Empty;

        public bool TemAnterior => PaginaAtual > 1;
        public bool TemProxima => PaginaAtual < TotalPaginas;
    }

    public class ListaPaginadaViewModel<T> : PaginacaoViewModel
    {
        public List<T> Itens { get; set; } = new List<T>();
    }
}
