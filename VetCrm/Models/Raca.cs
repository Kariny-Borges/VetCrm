namespace VetCrm.Models
{
    public class Raca
    {
        public int Id { get; set; }
        
        public string Nome { get; set; }

        public int EspecieId { get; set; }

        public Especie? Especie { get; set; } = null!;
    }
}