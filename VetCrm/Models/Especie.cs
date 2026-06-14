namespace VetCrm.Models
{
    public class Especie : EntidadeBase
    {
        public string Nome { get; set; }

        public List<Raca> Racas { get; set; } = new List<Raca>();
    }
}