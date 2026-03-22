using VetCrm.Models;

public class Veterinario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CRMV { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
    public List<Contato> Contatos { get; set; } = new List<Contato>();
    public List<Consulta> Consultas { get; set; } = new List<Consulta>();
}