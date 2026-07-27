namespace VetCrm.Models
{
    // Liga um paciente a um exame do catálogo (tabela Exames), com data e resultado.
    // Mesmo padrão do PacienteVacina: é a "ponte" entre o paciente e o catálogo.
    public class PacienteExame : EntidadeBase
    {
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int ExameId { get; set; }            // qual exame do catálogo foi pedido
        public Exame? Exame { get; set; }

        public int? ProntuarioId { get; set; }      // de qual atendimento veio
        public Prontuario? Prontuario { get; set; }

        public DateTime DataSolicitacao { get; set; }
        public string? Resultado { get; set; }      // opcional: preenche quando o exame fica pronto
    }
}
