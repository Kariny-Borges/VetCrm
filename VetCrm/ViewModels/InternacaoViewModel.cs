namespace VetCrm.ViewModels
{
    public class InternacaoViewModel : ViewModelEntidadeBase
    {
        public int PacienteId { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}