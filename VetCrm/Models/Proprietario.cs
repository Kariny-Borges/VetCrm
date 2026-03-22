using VetCrm.Models;

public class Proprietario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public int EnderecoId { get; set; }
    public Endereco Endereco { get; set; }
    public List<Contato> Contatos { get; set; } = new List<Contato>();
    public List<Paciente> Pacientes { get; set; } = new List<Paciente>();
}