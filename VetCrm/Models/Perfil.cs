namespace VetCrm.Models
{
    public class Perfil
    {
        public PerfilUsuario Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public List<Usuario> Usuarios { get; set; } = new();
    }
}
