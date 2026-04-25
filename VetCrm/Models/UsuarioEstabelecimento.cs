namespace VetCrm.Models
{
    public class UsuarioEstabelecimento
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int EstabelecimentoId { get; set; }
        public Estabelecimento? Estabelecimento { get; set; }

        public DateTime DataVinculo { get; set; } = DateTime.Now;
    }
}