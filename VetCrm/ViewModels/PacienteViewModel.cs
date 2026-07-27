namespace VetCrm.ViewModels
{
    public class PacienteViewModel : ViewModelEntidadeBase
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Sexo { get; set; }
        public decimal Peso { get; set; }
        public DateTime DataCadastro { get; set; }

        public int ProprietarioId { get; set; }
        public int EspecieId { get; set; }
        public int RacaId { get; set; }
    }
}