namespace VetCrm.ViewModels
{
    public class VacinaViewModel : ViewModelEntidadeBase
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public DateTime Validade { get; set; }
        public string Fabricante { get; set; } = string.Empty;
    }
}