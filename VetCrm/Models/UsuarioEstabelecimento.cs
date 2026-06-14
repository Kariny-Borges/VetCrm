namespace VetCrm.Models
{
    public class UsuarioEstabelecimento : EntidadeBase
    {

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int EstabelecimentoId { get; set; }
        public Estabelecimento? Estabelecimento { get; set; }
    }
}