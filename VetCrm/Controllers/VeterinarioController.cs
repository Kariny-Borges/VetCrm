using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class VeterinarioController : Controller
    {
        private readonly VetCrmContext _context;

        public VeterinarioController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Veterinario
        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Veterinarios
                .Include(v => v.Especialidade)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(v => v.Nome.Contains(busca) || v.CRMV.Contains(busca) || v.Especialidade.Nome.Contains(busca));
            }

            ViewData["BuscaAtual"] = busca;
            return View(await query.ToListAsync());
        }

        // GET: Veterinario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios
                .Include(v => v.Especialidade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinario == null)
            {
                return NotFound();
            }

            return View(veterinario);
        }

        // GET: Veterinario/Create
        public IActionResult Create()
        {
            ViewData["EspecialidadeId"] = new SelectList(_context.Especialidades, "Id", "Nome");
            return View();
        }

        // POST: Veterinario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VeterinarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var veterinario = new Veterinario
                {
                    Nome = model.Nome,
                    CRMV = model.CRMV,
                    EspecialidadeId = model.EspecialidadeId
                };

                _context.Add(veterinario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecialidadeId"] = new SelectList(_context.Especialidades, "Id", "Nome", model.EspecialidadeId);
            return View(model);
        }

        // GET: Veterinario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios.FindAsync(id);
            if (veterinario == null)
            {
                return NotFound();
            }

            var model = new VeterinarioViewModel
            {
                Id = veterinario.Id,
                Nome = veterinario.Nome,
                CRMV = veterinario.CRMV,
                EspecialidadeId = veterinario.EspecialidadeId
            };

            ViewData["EspecialidadeId"] = new SelectList(_context.Especialidades, "Id", "Nome", model.EspecialidadeId);
            return View(model);
        }

        // POST: Veterinario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VeterinarioViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var veterinario = await _context.Veterinarios.FindAsync(id);
                    if (veterinario == null)
                    {
                        return NotFound();
                    }

                    veterinario.Nome = model.Nome;
                    veterinario.CRMV = model.CRMV;
                    veterinario.EspecialidadeId = model.EspecialidadeId;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeterinarioExists(model.Id))
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
            ViewData["EspecialidadeId"] = new SelectList(_context.Especialidades, "Id", "Nome", model.EspecialidadeId);
            return View(model);
        }

        // GET: Veterinario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios
                .Include(v => v.Especialidade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinario == null)
            {
                return NotFound();
            }

            return View(veterinario);
        }

        // POST: Veterinario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veterinario = await _context.Veterinarios.FindAsync(id);
            if (veterinario != null)
            {
                _context.Veterinarios.Remove(veterinario);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir este veterinário porque ele possui consultas vinculadas.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VeterinarioExists(int id)
        {
            return _context.Veterinarios.Any(e => e.Id == id);
        }
    }
}