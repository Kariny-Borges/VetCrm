using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;

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

            ViewData["BuscaAtual"] = busca;
            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var raca = await _context.Racas.Include(r => r.Especie).FirstOrDefaultAsync(m => m.Id == id);
            if (raca == null) return NotFound();
            return View(raca);
        }

        public IActionResult Create()
        {
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,EspecieId")] Raca raca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", raca.EspecieId);
            return View(raca);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var raca = await _context.Racas.FindAsync(id);
            if (raca == null) return NotFound();
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", raca.EspecieId);
            return View(raca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,EspecieId")] Raca raca)
        {
            if (id != raca.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacaExists(raca.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", raca.EspecieId);
            return View(raca);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var raca = await _context.Racas.Include(r => r.Especie).FirstOrDefaultAsync(m => m.Id == id);
            if (raca == null) return NotFound();
            return View(raca);
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