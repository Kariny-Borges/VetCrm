namespace VetCrm.Models
{
    public class Internacao : EntidadeBase
    {
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string? Observacoes { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
    }
}
