using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;

namespace VetCrm.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly VetCrmContext _context;

        public ProdutoController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return View(produtos);
        }
    }
}
