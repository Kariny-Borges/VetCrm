namespace VetCrm.ViewModels
{
    public class PacienteVacinaViewModel : ViewModelEntidadeBase
    {
        public int PacienteId { get; set; }
        public int VacinaId { get; set; }
        public DateTime DataAplicacao { get; set; }
        public DateTime DataProximaDose { get; set; }
    }
}