using System.Collections.Generic;

namespace VetCrm.Models.ViewModels
{
    // "Bandeja" que leva tudo o que a tela de atendimento precisa mostrar de uma vez.
    // Não vira tabela no banco: existe só para servir a View.
    public class AtendimentoViewModel
    {
        // A consulta que está sendo atendida (traz o paciente junto).
        public Consulta Consulta { get; set; }

        // O prontuário desta consulta (o formulário de evolução que o vet preenche).
        public Prontuario Prontuario { get; set; }

        // A caixinha "finalizar consulta".
        public bool FinalizarConsulta { get; set; }

        // Histórico do paciente.
        public List<Consulta> HistoricoConsultas { get; set; } = new List<Consulta>();
        public List<PacienteVacina> HistoricoVacinas { get; set; } = new List<PacienteVacina>();
    }
}
