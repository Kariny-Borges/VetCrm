namespace VetCrm.Models
{
    public class Usuario : PessoaFisica
    {
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public PerfilUsuario Perfil { get; set; }
        public Perfil? PerfilNavegacao { get; set; }

        public List<UsuarioEstabelecimento> UsuarioEstabelecimentos { get; set; } = new();
    }
}