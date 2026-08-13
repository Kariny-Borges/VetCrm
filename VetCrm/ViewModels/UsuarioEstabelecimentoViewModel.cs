namespace VetCrm.ViewModels
{
    public class UsuarioEstabelecimentoViewModel : ViewModelEntidadeBase
    {
        public int UsuarioId { get; set; }

        public int EstabelecimentoId { get; set; }
        public EstabelecimentoViewModel? Estabelecimento { get; set; }
    }
}
