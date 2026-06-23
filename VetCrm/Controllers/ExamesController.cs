using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;

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
        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Exames.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(e => e.Nome.Contains(busca));
            }

            ViewData["BuscaAtual"] = busca;
            return View(await query.ToListAsync());
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

            return View(exame);
        }

        // GET: Exames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exames/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Exame exame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exame);
        }

        // GET: Exames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exame = await _context.Exames.FindAsync(id);
            if (exame == null)
            {
                return NotFound();
            }
            return View(exame);
        }

        // POST: Exames/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Exame exame)
        {
            if (id != exame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExameExists(exame.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(exame);
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

            return View(exame);
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
