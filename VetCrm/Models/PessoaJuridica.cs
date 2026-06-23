namespace VetCrm.Models
{
    public abstract class PessoaJuridica : Pessoa
    {
        public string CNPJ { get; set; } = string.Empty;
    }
}