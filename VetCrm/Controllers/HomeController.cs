using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VetCrm.Models;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    [Authorize] // exige usuário logado para qualquer action deste controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // A raiz do site (/) cai aqui.
        // Visitante vê a landing page; quem já entrou vê o painel.
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return View("Dashboard");
            }

            return View("Landing");
        }

        [AllowAnonymous] // Privacy continua acessível sem login
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous] // Termos de uso também acessível sem login
        public IActionResult Termos()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}