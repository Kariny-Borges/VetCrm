namespace VetCrm.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Sexo { get; set; }
        public decimal Peso { get; set; }
        public DateTime DataCadastro { get; set; }

        public int ProprietarioId { get; set; }
        public Proprietario Proprietario { get; set; }

        public int EspecieId { get; set; }
        public Especie Especie { get; set; }

        public int RacaId { get; set; }
        public Raca Raca { get; set; }

        public List<Consulta> Consultas { get; set; }
        public List<Prontuario> Prontuarios { get; set; }
        public List<PacienteVacina> PacienteVacinas { get; set; }
    }
}