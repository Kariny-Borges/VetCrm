namespace VetCrm.Models
{
    public class Raca : EntidadeBase
    {
        public string Nome { get; set; }

        public int EspecieId { get; set; }

        public Especie? Especie { get; set; } = null!;
    }
}