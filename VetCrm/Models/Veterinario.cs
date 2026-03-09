namespace VetCrm.Models
{
    public class Veterinario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CRMV { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Especialidade { get; set; }

        public List<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}