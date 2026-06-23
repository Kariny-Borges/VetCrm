namespace VetCrm.Models
{
    public class Estabelecimento : PessoaJuridica
    {
        public List<UsuarioEstabelecimento> UsuarioEstabelecimentos { get; set; } = new();
    }
}