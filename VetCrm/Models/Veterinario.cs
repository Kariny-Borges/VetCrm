using VetCrm.Models;

public class Veterinario : PessoaFisica
{
    public string CRMV { get; set; } = string.Empty;

    public int? EspecialidadeId { get; set; }
    public Especialidade? Especialidade { get; set; }

    public List<Consulta> Consultas { get; set; } = new List<Consulta>();
}