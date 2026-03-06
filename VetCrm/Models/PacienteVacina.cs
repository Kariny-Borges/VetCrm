namespace VetCrm.Models
{
    public class PacienteVacina
    {
        public int Id { get; set; }

        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        public int VacinaId { get; set; }
        public Vacina Vacina { get; set; }

        public DateTime DataAplicacao { get; set; }
        public DateTime DataProximaDose { get; set; }
    }
}