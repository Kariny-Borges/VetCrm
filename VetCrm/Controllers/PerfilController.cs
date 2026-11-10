using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetCrm.Data;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly VetCrmContext _context;

        public PerfilController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Perfil - mostra os dados de quem está logado
        public async Task<IActionResult> Index()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            var usuarioViewModel = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Login = usuario.Login
            };

            return View(usuarioViewModel);
        }

        // GET: Perfil/AlterarSenha - abre o formulario vazio
        public IActionResult AlterarSenha()
        {
            return View();
        }

        // POST: Perfil/AlterarSenha - confere a senha atual e salva a nova
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null) return NotFound();

                // ATENÇÃO: senha em texto puro, igual ao Login (em produção usar hash)
                if (usuario.Senha != model.SenhaAtual)
                {
                    ModelState.AddModelError(nameof(model.SenhaAtual), "Senha atual incorreta.");
                    return View(model);
                }

                usuario.Senha = model.NovaSenha;
                await _context.SaveChangesAsync();

                TempData["Mensagem"] = "Senha alterada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}