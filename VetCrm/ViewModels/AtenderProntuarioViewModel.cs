namespace VetCrm.ViewModels
{
    public class AtenderProntuarioViewModel : ViewModelEntidadeBase
    {
        public int ConsultaId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
    }
}