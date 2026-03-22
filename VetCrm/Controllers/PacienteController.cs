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
    public class PacienteController : Controller
    {
        private readonly VetCrmContext _context;

        public PacienteController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vetCrmContext = _context.Pacientes
                .Include(p => p.Especie)
                .Include(p => p.Proprietario)
                .Include(p => p.Raca);
            return View(await vetCrmContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.Especie)
                .Include(p => p.Proprietario)
                .Include(p => p.Raca)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (paciente == null) return NotFound();

            return View(paciente);
        }

        public IActionResult Create()
        {
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome");
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome");
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Idade,Sexo,Peso,DataCadastro,ProprietarioId,EspecieId,RacaId")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", paciente.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome", paciente.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome", paciente.RacaId);
            return View(paciente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", paciente.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome", paciente.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome", paciente.RacaId);
            return View(paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Idade,Sexo,Peso,DataCadastro,ProprietarioId,EspecieId,RacaId")] Paciente paciente)
        {
            if (id != paciente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", paciente.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome", paciente.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome", paciente.RacaId);
            return View(paciente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.Especie)
                .Include(p => p.Proprietario)
                .Include(p => p.Raca)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (paciente == null) return NotFound();

            return View(paciente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null) _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}