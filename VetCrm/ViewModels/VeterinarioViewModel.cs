namespace VetCrm.ViewModels
{
    public class VeterinarioViewModel : PessoaViewModel
    {
        public string CRMV { get; set; } = string.Empty;
        public int? EspecialidadeId { get; set; }
        public EspecialidadeViewModel? Especialidade { get; set; }
    }
}