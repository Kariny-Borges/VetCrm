using VetCrm.Models;

namespace VetCrm.ViewModels
{
    public class UsuarioViewModel : PessoaFisicaViewModel
    {
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public PerfilUsuario Perfil { get; set; }

        public int? EnderecoId { get; set; }
        public EnderecoViewModel? Endereco { get; set; }
    }
}