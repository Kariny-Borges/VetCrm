namespace VetCrm.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Observacoes { get; set; }

        public TipoConsulta TipoConsulta { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int VeterinarioId { get; set; }
        public Veterinario? Veterinario { get; set; }
        public int ProntuaarioId { get; set; }

        public Prontuario? Prontuario { get; set; }
    }
}