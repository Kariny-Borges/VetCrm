using VetCrm.Models;

namespace VetCrm.ViewModels
{
    public class ConsultaViewModel : ViewModelEntidadeBase
    {
        public DateTime DataConsulta { get; set; }
        public SituacaoConsulta Situacao { get; set; }
        public string Observacoes { get; set; }

        public int TipoConsultaId { get; set; }
        public TipoConsultaViewModel? TipoConsulta { get; set; }

        public int PacienteId { get; set; }
        public PacienteViewModel? Paciente { get; set; }

        public int VeterinarioId { get; set; }
        public VeterinarioViewModel? Veterinario { get; set; }

        public string Resumo => $"{DataConsulta:dd/MM/yyyy} - {Paciente?.Nome}";
    }
}