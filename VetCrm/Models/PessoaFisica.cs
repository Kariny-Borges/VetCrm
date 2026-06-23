namespace VetCrm.Models
{
    public abstract class PessoaFisica : Pessoa
    {
        public string CPF { get; set; } = string.Empty;
    }
}