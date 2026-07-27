namespace VetCrm.ViewModels
{
    public class EditarVacinaViewModel : ViewModelEntidadeBase
    {
        public int VacinaId { get; set; }
        public DateTime DataAplicacao { get; set; }
        public DateTime DataProximaDose { get; set; }
    }
}