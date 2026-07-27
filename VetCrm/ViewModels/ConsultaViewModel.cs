namespace VetCrm.ViewModels
{
    public class ConsultaViewModel : ViewModelEntidadeBase
    {
        public DateTime DataConsulta { get; set; }
        public string Observacoes { get; set; }

        public int TipoConsultaId { get; set; }
        public int PacienteId { get; set; }
        public int VeterinarioId { get; set; }
    }
}