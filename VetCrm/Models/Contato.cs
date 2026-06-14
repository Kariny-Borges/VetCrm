namespace VetCrm.Models
{
    public class Contato : EntidadeBase
    {
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
}