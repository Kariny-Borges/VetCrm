using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class RacaController : Controller
    {
        private readonly VetCrmContext _context;

        public RacaController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Racas.Include(r => r.Especie).AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(r => r.Nome.Contains(busca) || r.Especie.Nome.Contains(busca));
            }

            var racas = await query.ToListAsync();
            var racasViewModel = racas.Select(r => new RacaViewModel
            {
                Id = r.Id,
                Nome = r.Nome,
                EspecieId = r.EspecieId,
                Especie = r.Especie == null ? null : new EspecieViewModel
                {
                    Id = r.Especie.Id,
                    Nome = r.Especie.Nome
                }
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(racasViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var raca = await _context.Racas.Include(r => r.Especie).FirstOrDefaultAsync(m => m.Id == id);
            if (raca == null) return NotFound();

            var racaViewModel = new RacaViewModel
            {
                Id = raca.Id,
                Nome = raca.Nome,
                EspecieId = raca.EspecieId,
                Especie = raca.Especie == null ? null : new EspecieViewModel
                {
                    Id = raca.Especie.Id,
                    Nome = raca.Especie.Nome
                }
            };

            return View(racaViewModel);
        }

        public IActionResult Create()
        {
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RacaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var raca = new Raca
                {
                    Nome = model.Nome,
                    EspecieId = model.EspecieId
                };

                _context.Add(raca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", model.EspecieId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var raca = await _context.Racas.FindAsync(id);
            if (raca == null) return NotFound();

            var model = new RacaViewModel
            {
                Id = raca.Id,
                Nome = raca.Nome,
                EspecieId = raca.EspecieId
            };

            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", model.EspecieId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RacaViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var raca = await _context.Racas.FindAsync(id);
                    if (raca == null) return NotFound();

                    raca.Nome = model.Nome;
                    raca.EspecieId = model.EspecieId;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacaExists(model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", model.EspecieId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var raca = await _context.Racas.Include(r => r.Especie).FirstOrDefaultAsync(m => m.Id == id);
            if (raca == null) return NotFound();

            var racaViewModel = new RacaViewModel
            {
                Id = raca.Id,
                Nome = raca.Nome,
                EspecieId = raca.EspecieId,
                Especie = raca.Especie == null ? null : new EspecieViewModel
                {
                    Id = raca.Especie.Id,
                    Nome = raca.Especie.Nome
                }
            };

            return View(racaViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raca = await _context.Racas.FindAsync(id);
            if (raca != null) _context.Racas.Remove(raca);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir esta raça porque existem pacientes vinculados a ela.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RacaExists(int id)
        {
            return _context.Racas.Any(e => e.Id == id);
        }
    }
}