namespace VetCrm.Models
{
    public abstract class Pessoa : EntidadeBase
    {
        public string Nome { get; set; } = string.Empty;
        public TipoPessoa TipoPessoa { get; set; }

        public int? EnderecoId { get; set; }
        public Endereco? Endereco { get; set; }

        public List<Contato> Contatos { get; set; } = new List<Contato>();
    }
}
