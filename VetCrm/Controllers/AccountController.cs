using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    // [AllowAnonymous] permite acessar este controller mesmo sem estar logado
    // (caso contrário a tela de login ficaria inacessível!).
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly VetCrmContext _context;

        public AccountController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: /Account/Login - mostra o formulário
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login - recebe os dados do formulário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            // Busca usuário pelo login (poderia ser email também).
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == model.Login);

            // ATENÇÃO: senha em texto puro só por simplicidade educacional.
            // Em produção use BCrypt: BCrypt.Net.BCrypt.Verify(model.Senha, usuario.Senha)
            if (usuario == null || usuario.Senha != model.Senha)
            {
                ModelState.AddModelError(string.Empty, "Login ou senha inválidos.");
                return View(model);
            }

            // Claims = pedacinhos de informação sobre o usuário que ficam guardados no cookie.
            // Você pode ler isso depois com User.Identity.Name, User.FindFirst(...), etc.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Cookie de sessão: expira ao fechar o navegador, forçando novo login.
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false
            };

            // Aqui o ASP.NET cria o cookie e manda pro navegador.
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            // Volta pra página que ele tentou acessar antes (ou pra Home).
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout - apaga o cookie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: /Account/EsqueciSenha
        public IActionResult EsqueciSenha()
        {
            return View();
        }

        // POST: /Account/EsqueciSenha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EsqueciSenha(EsqueciSenhaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            // Por segurança, mostre a mesma mensagem mesmo se o email não existir.
            // Assim ninguém descobre quais emails estão cadastrados sondando o formulário.
            if (usuario != null)
            {
                usuario.Senha = model.NovaSenha; // em produção: hash aqui também
                await _context.SaveChangesAsync();
            }

            TempData["Mensagem"] = "Se o email estiver cadastrado, a senha foi atualizada. Faça login com a nova senha.";
            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/AccessDenied - mostrada quando usuário logado tenta acessar algo sem permissão
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
