namespace VetCrm.Models;

public class Proprietario : Pessoa   
{
    public string CPF { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public List<Paciente> Pacientes { get; set; } = new List<Paciente>();
}