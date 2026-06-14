using System.Collections.Generic;

namespace VetCrm.Models

{
    public class Vacina : EntidadeBase
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Lote { get; set; } = string.Empty;
        public DateTime Validade { get; set; }
        public string Fabricante { get; set; } = string.Empty;

        public List<PacienteVacina> PacienteVacinas { get; set; } = new List<PacienteVacina>();
    }
}