namespace VetCrm.Models
{
    public class Prontuario : EntidadeBase
    {
        public string Descricao { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamento { get; set; }
        public DateTime DataRegistro { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int ConsultaId { get; set; }
        public Consulta? Consulta { get; set; }
    }
}