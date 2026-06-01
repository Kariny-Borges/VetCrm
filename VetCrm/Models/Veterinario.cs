using VetCrm.Models;

public class Veterinario : Pessoa   
{
    public string CRMV { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
    public List<Consulta> Consultas { get; set; } = new List<Consulta>();
}