using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VetCrm.ViewModels
{
    // "Bandeja" que leva tudo o que a tela de atendimento precisa mostrar de uma vez.
    // Não vira tabela no banco: existe só para servir a View.
    public class AtendimentoViewModel
    {
        // A consulta que está sendo atendida (traz o paciente junto).
        public ConsultaViewModel Consulta { get; set; } = new ConsultaViewModel();

        // O prontuário desta consulta (o formulário de evolução que o vet preenche).
        public AtenderProntuarioViewModel Prontuario { get; set; } = new AtenderProntuarioViewModel();

        // A caixinha "finalizar consulta".
        public bool FinalizarConsulta { get; set; }

        // Histórico do paciente. Só exibição.
        public List<ConsultaViewModel> HistoricoConsultas { get; set; } = new List<ConsultaViewModel>();
        public List<PacienteVacinaViewModel> HistoricoVacinas { get; set; } = new List<PacienteVacinaViewModel>();
        public List<TratamentoViewModel> HistoricoTratamentos { get; set; } = new List<TratamentoViewModel>();

        // Formulário pra lançar um novo tratamento na lista (fica em branco até o vet preencher).
        public NovoTratamentoViewModel NovoTratamento { get; set; } = new NovoTratamentoViewModel();

        // Lista de exames do paciente (com o exame do catálogo já carregado).
        public List<PacienteExameViewModel> HistoricoExames { get; set; } = new List<PacienteExameViewModel>();

        // Formulário pra pedir um novo exame (fica em branco até o vet escolher).
        public NovoExameViewModel NovoExame { get; set; } = new NovoExameViewModel();

        // Opções do dropdown de exames (vem do catálogo Exame).
        public List<SelectListItem> ExamesDisponiveis { get; set; } = new List<SelectListItem>();

        // Formulário pra aplicar uma nova vacina (fica em branco até o vet escolher).
        public NovaVacinaViewModel NovaVacina { get; set; } = new NovaVacinaViewModel();

        // Opções do dropdown de vacinas (vem do catálogo Vacina).
        public List<SelectListItem> VacinasDisponiveis { get; set; } = new List<SelectListItem>();

        // Se a consulta já foi Realizada, a tela vira só leitura (não deixa mais editar).
        public bool SomenteLeitura { get; set; }
    }
}