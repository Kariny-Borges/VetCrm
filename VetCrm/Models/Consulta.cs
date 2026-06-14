using System.ComponentModel.DataAnnotations.Schema;

namespace VetCrm.Models
{
    public class Consulta : EntidadeBase
    {
        public DateTime DataConsulta { get; set; }
        public string Observacoes { get; set; }

        public int TipoConsultaId { get; set; }
        public TipoConsulta? TipoConsulta { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int VeterinarioId { get; set; }
        public Veterinario? Veterinario { get; set; }

        public Prontuario? Prontuario { get; set; }

        // Etiqueta legível da consulta (ex: "10/06/2026 - Rex"). Não vira coluna no banco.
        [NotMapped]
        public string Resumo => $"{DataConsulta:dd/MM/yyyy} - {Paciente?.Nome}";
    }
}