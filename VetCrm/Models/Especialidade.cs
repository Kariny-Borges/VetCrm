namespace VetCrm.Models
{
    public class Especialidade : EntidadeBase
    {
        public string Nome { get; set; } = string.Empty;

        public List<Veterinario> Veterinarios { get; set; } = new List<Veterinario>();
    }
}
