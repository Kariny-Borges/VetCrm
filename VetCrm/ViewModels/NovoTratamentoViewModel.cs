namespace VetCrm.ViewModels
{
    public class NovoTratamentoViewModel
    {
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}