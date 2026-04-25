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
    public class PacienteVacinaController : Controller
    {
        private readonly VetCrmContext _context;

        public PacienteVacinaController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: PacienteVacina
        public async Task<IActionResult> Index()
        {
            var vetCrmContext = _context.PacienteVacinas.Include(p => p.Paciente).Include(p => p.Vacina);
            return View(await vetCrmContext.ToListAsync());
        }

        // GET: PacienteVacina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteVacina = await _context.PacienteVacinas
                .Include(p => p.Paciente)
                .Include(p => p.Vacina)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacienteVacina == null)
            {
                return NotFound();
            }

            return View(pacienteVacina);
        }

        // GET: PacienteVacina/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id");
            ViewData["VacinaId"] = new SelectList(_context.Vacinas, "Id", "Id");
            return View();
        }

        // POST: PacienteVacina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PacienteId,VacinaId,DataAplicacao,DataProximaDose")] PacienteVacina pacienteVacina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pacienteVacina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id", pacienteVacina.PacienteId);
            ViewData["VacinaId"] = new SelectList(_context.Vacinas, "Id", "Id", pacienteVacina.VacinaId);
            return View(pacienteVacina);
        }

        // GET: PacienteVacina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteVacina = await _context.PacienteVacinas.FindAsync(id);
            if (pacienteVacina == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id", pacienteVacina.PacienteId);
            ViewData["VacinaId"] = new SelectList(_context.Vacinas, "Id", "Id", pacienteVacina.VacinaId);
            return View(pacienteVacina);
        }

        // POST: PacienteVacina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PacienteId,VacinaId,DataAplicacao,DataProximaDose")] PacienteVacina pacienteVacina)
        {
            if (id != pacienteVacina.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pacienteVacina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteVacinaExists(pacienteVacina.Id))
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
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id", pacienteVacina.PacienteId);
            ViewData["VacinaId"] = new SelectList(_context.Vacinas, "Id", "Id", pacienteVacina.VacinaId);
            return View(pacienteVacina);
        }

        // GET: PacienteVacina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteVacina = await _context.PacienteVacinas
                .Include(p => p.Paciente)
                .Include(p => p.Vacina)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacienteVacina == null)
            {
                return NotFound();
            }

            return View(pacienteVacina);
        }

        // POST: PacienteVacina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pacienteVacina = await _context.PacienteVacinas.FindAsync(id);
            if (pacienteVacina != null)
            {
                _context.PacienteVacinas.Remove(pacienteVacina);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir este registro de vacinação.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PacienteVacinaExists(int id)
        {
            return _context.PacienteVacinas.Any(e => e.Id == id);
        }
    }
}
