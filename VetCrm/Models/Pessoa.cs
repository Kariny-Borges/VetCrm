namespace VetCrm.Models
{
    public abstract class Pessoa   
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public int? EnderecoId { get; set; }
        public Endereco? Endereco { get; set; }

        public List<Contato> Contatos { get; set; } = new List<Contato>();
    }
}
