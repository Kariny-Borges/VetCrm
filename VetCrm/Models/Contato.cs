namespace VetCrm.Models
{
    public class Contato
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; 
        public string Valor { get; set; } = string.Empty;
    }
}