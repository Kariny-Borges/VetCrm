namespace VetCrm.Models;

public class Proprietario : PessoaFisica
{
    public DateTime DataCadastro { get; set; }
    public List<Paciente> Pacientes { get; set; } = new List<Paciente>();
}