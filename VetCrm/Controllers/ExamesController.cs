using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class ExamesController : Controller
    {
        private readonly VetCrmContext _context;

        public ExamesController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Exames
        public async Task<IActionResult> Index(string busca, int pagina = 1)
        {
            var query = _context.Exames.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(e => e.Nome.Contains(busca));
            }

            var itensPorPagina = 10;
            var totalItens = await query.CountAsync();

            var exames = await query
                .OrderBy(e => e.Id)
                .Skip((pagina - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToListAsync();
            var examesViewModel = exames.Select(e => new ExameViewModel
            {
                Id = e.Id,
                Nome = e.Nome
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(new ListaPaginadaViewModel<ExameViewModel>
            {
                Itens = examesViewModel,
                PaginaAtual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalItens / (double)itensPorPagina),
                Busca = busca
            });
        }

        // GET: Exames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exame = await _context.Exames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exame == null)
            {
                return NotFound();
            }

            var exameViewModel = new ExameViewModel
            {
                Id = exame.Id,
                Nome = exame.Nome
            };

            return View(exameViewModel);
        }

        // GET: Exames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exames/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exame = new Exame
                {
                    Nome = model.Nome
                };
                _context.Add(exame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Exames/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var exame = await _context.Exames.FindAsync(id);
            if (exame == null) return NotFound();

            var exameViewModel = new ExameViewModel
            {
                Id = exame.Id,
                Nome = exame.Nome
            };

            return View(exameViewModel);
        }

        // POST: Exames/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExameViewModel exameViewModel)
        {
            if (ModelState.IsValid)
            {
                var exame = await _context.Exames.FindAsync(id);
                if (exame == null) return NotFound();

                exame.Nome = exameViewModel.Nome;
                _context.Exames.Update(exame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exameViewModel);
        }

        // GET: Exames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exame = await _context.Exames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exame == null)
            {
                return NotFound();
            }

            var exameViewModel = new ExameViewModel
            {
                Id = exame.Id,
                Nome = exame.Nome
            };

            return View(exameViewModel);
        }

        // POST: Exames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exame = await _context.Exames.FindAsync(id);
            if (exame != null)
            {
                _context.Exames.Remove(exame);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExameExists(int id)
        {
            return _context.Exames.Any(e => e.Id == id);
        }
    }
}