namespace VetCrm.ViewModels
{
    public class ProntuarioViewModel : ViewModelEntidadeBase
    {
        public string Descricao { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamento { get; set; } = string.Empty;
        public DateTime DataRegistro { get; set; }

        public int PacienteId { get; set; }
        public PacienteViewModel? Paciente { get; set; }

        public int ConsultaId { get; set; }
        public ConsultaViewModel? Consulta { get; set; }
    }
}