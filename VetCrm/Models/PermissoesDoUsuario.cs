using System.Linq;
using System.Security.Claims;

namespace VetCrm.Models
{
    public static class PermissoesDoUsuario
    {
        // Diz se o usuário logado tem uma permissão.
        public static bool Pode(this ClaimsPrincipal user, string permissao)
        {
            return user.HasClaim(Permissoes.TipoClaim, permissao);
        }

        // Diz se o usuário tem pelo menos uma das permissões da lista.
        // Usado para esconder um menu inteiro quando nenhum item dele aparece.
        public static bool PodeAlgum(this ClaimsPrincipal user, params string[] permissoes)
        {
            return permissoes.Any(p => user.Pode(p));
        }
    }
}
