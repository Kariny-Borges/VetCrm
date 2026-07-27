using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VetCrm.Models;

namespace VetCrm.ViewModels
{
    // "Bandeja" que leva tudo o que a tela de atendimento precisa mostrar de uma vez.
    // Não vira tabela no banco: existe só para servir a View.
    public class AtendimentoViewModel
    {
        // A consulta que está sendo atendida (traz o paciente junto).
        // Só exibição: continua sendo a entidade do EF.
        public Consulta Consulta { get; set; }

        // O prontuário desta consulta (o formulário de evolução que o vet preenche).
        public AtenderProntuarioViewModel Prontuario { get; set; } = new AtenderProntuarioViewModel();

        // A caixinha "finalizar consulta".
        public bool FinalizarConsulta { get; set; }

        // Histórico do paciente. Só exibição: continuam sendo entidades do EF.
        public List<Consulta> HistoricoConsultas { get; set; } = new List<Consulta>();
        public List<PacienteVacina> HistoricoVacinas { get; set; } = new List<PacienteVacina>();
        public List<Tratamento> HistoricoTratamentos { get; set; } = new List<Tratamento>();

        // Formulário pra lançar um novo tratamento na lista (fica em branco até o vet preencher).
        public NovoTratamentoViewModel NovoTratamento { get; set; } = new NovoTratamentoViewModel();

        // Lista de exames do paciente (com o exame do catálogo já carregado).
        public List<PacienteExame> HistoricoExames { get; set; } = new List<PacienteExame>();

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