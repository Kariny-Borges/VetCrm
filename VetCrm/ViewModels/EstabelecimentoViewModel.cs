// ViewModels/EstabelecimentoViewModel.cs
namespace VetCrm.ViewModels
{
    public class EstabelecimentoViewModel : PessoaJuridicaViewModel
    {
        public int? EnderecoId { get; set; }
        public EnderecoViewModel? Endereco { get; set; }
    }
}