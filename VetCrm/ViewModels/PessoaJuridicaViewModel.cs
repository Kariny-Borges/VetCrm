namespace VetCrm.ViewModels
{
    public abstract class PessoaJuridicaViewModel : PessoaViewModel
    {
        public string CNPJ { get; set; } = string.Empty;
    }
}