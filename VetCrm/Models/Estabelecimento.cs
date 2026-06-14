namespace VetCrm.Models
{
    public class Estabelecimento : Pessoa
    {
        public string CNPJ { get; set; } = string.Empty;

        public List<UsuarioEstabelecimento> UsuarioEstabelecimentos { get; set; } = new();
    }
}