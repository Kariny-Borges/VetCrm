using System.ComponentModel.DataAnnotations.Schema;

namespace VetCrm.Models
{
    public class Consulta : EntidadeBase
    {
        public DateTime DataConsulta { get; set; }
        public SituacaoConsulta Situacao { get; set; } = SituacaoConsulta.Agendada;
        public string Observacoes { get; set; }

        public int TipoConsultaId { get; set; }
        public TipoConsulta? TipoConsulta { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int VeterinarioId { get; set; }
        public Veterinario? Veterinario { get; set; }

        public Prontuario? Prontuario { get; set; }

        [NotMapped]
        public string Resumo => $"{DataConsulta:dd/MM/yyyy} - {Paciente?.Nome}";
    }
}