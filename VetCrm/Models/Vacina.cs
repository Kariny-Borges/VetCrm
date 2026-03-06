using System.Collections.Generic;

namespace VetCrm.Models

{
    public class Vacina
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public List<PacienteVacina> PacienteVacinas { get; set; }
    }
}