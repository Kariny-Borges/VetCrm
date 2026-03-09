namespace VetCrm.Models
{
    public class Especie
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public List<Raca> Racas { get; set; } = new List<Raca>();
    }
}