namespace VetCrm.ViewModels
{
    public class PacienteExameViewModel : ViewModelEntidadeBase
    {
        public int ExameId { get; set; }
        public ExameViewModel? Exame { get; set; }
        public string? Resultado { get; set; }
    }
}