namespace VetCrm.Models
{
    public enum TipoPessoa
    {
        PF = 1,
        PJ = 2
    }

    public enum PerfilUsuario
    {
        Recepcao = 1,
        Administrativo = 2,
        Financeiro = 3,
        Gerente = 4
    }

    public class Usuario : Pessoa   
    {
        public string Documento { get; set; } = string.Empty; 
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public TipoPessoa TipoPessoa { get; set; }
        public PerfilUsuario Perfil { get; set; }

        public List<UsuarioEstabelecimento> UsuarioEstabelecimentos { get; set; } = new();
    }
}