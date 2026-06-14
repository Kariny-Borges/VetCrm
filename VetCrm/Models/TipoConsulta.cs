namespace VetCrm.Models
{
    public class TipoConsulta : EntidadeBase
    {
        public string Nome { get; set; } = string.Empty;

        public List<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
