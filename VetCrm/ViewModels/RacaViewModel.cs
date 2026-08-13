namespace VetCrm.ViewModels
{
    public class RacaViewModel : ViewModelEntidadeBase
    {
        public string Nome { get; set; } = string.Empty;
        public int EspecieId { get; set; }
        public EspecieViewModel? Especie { get; set; }
    }
}