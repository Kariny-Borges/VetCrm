namespace VetCrm.Models
{
    public class Estabelecimento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;

        public int? EnderecoId { get; set; }
        public Endereco? Endereco { get; set; }

        public List<UsuarioEstabelecimento> UsuarioEstabelecimentos { get; set; } = new();
    }
}