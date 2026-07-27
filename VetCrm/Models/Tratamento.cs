namespace VetCrm.Models
{
    public class Tratamento : EntidadeBase
    {
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int? ProntuarioId { get; set; }
        public Prontuario? Prontuario { get; set; }
    }
}