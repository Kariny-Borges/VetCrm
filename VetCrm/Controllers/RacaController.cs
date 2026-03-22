using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: Raca
        public async Task<IActionResult> Index()
        {
            var vetCrmContext = _context.Racas.Include(r => r.Especie);
            return View(await vetCrmContext.ToListAsync());
        }

        // GET: Raca/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raca = await _context.Racas
                .Include(r => r.Especie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (raca == null)
            {
                return NotFound();
            }

            return View(raca);
        }

        // GET: Raca/Create
        public IActionResult Create()
        {
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Id");
            return View();
        }

        // POST: Raca/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Id", raca.EspecieId);
            return View(raca);
        }

        // GET: Raca/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raca = await _context.Racas.FindAsync(id);
            if (raca == null)
            {
                return NotFound();
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Id", raca.EspecieId);
            return View(raca);
        }

        // POST: Raca/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,EspecieId")] Raca raca)
        {
            if (id != raca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacaExists(raca.Id))
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
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Id", raca.EspecieId);
            return View(raca);
        }

        // GET: Raca/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raca = await _context.Racas
                .Include(r => r.Especie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (raca == null)
            {
                return NotFound();
            }

            return View(raca);
        }

        // POST: Raca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raca = await _context.Racas.FindAsync(id);
            if (raca != null)
            {
                _context.Racas.Remove(raca);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacaExists(int id)
        {
            return _context.Racas.Any(e => e.Id == id);
        }
    }
}
