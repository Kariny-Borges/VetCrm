namespace VetCrm.ViewModels
{
    public class EditarTratamentoViewModel : ViewModelEntidadeBase
    {
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}