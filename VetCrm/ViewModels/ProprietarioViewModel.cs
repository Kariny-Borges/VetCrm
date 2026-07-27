namespace VetCrm.ViewModels
{
    public class ProprietarioViewModel : PessoaFisicaViewModel
    {
        public DateTime DataCadastro { get; set; }

        public int? EnderecoId { get; set; }
        public EnderecoViewModel? Endereco { get; set; }
    }
}