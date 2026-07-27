namespace VetCrm.Models
{
    public class PacienteVacina : EntidadeBase
    {

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int VacinaId { get; set; }
        public Vacina? Vacina { get; set; }

        public int? ProntuarioId { get; set; }      // de qual atendimento veio (opcional)
        public Prontuario? Prontuario { get; set; }

        public DateTime DataAplicacao { get; set; }
        public DateTime DataProximaDose { get; set; }
    }
}