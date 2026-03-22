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

        // GET: Paciente
        public async Task<IActionResult> Index()
        {
            var vetCrmContext = _context.Pacientes.Include(p => p.Especie).Include(p => p.Proprietario).Include(p => p.Raca);
            return View(await vetCrmContext.ToListAsync());
        }

        // GET: Paciente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Especie)
                .Include(p => p.Proprietario)
                .Include(p => p.Raca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Paciente/Create
        public IActionResult Create()
        {
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Id");
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Id");
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Id");
            return View();
        }

        // POST: Paciente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Id", paciente.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Id", paciente.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Id", paciente.RacaId);
            return View(paciente);
        }

        // GET: Paciente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Id", paciente.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Id", paciente.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Id", paciente.RacaId);
            return View(paciente);
        }

        // POST: Paciente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Idade,Sexo,Peso,DataCadastro,ProprietarioId,EspecieId,RacaId")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
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
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Id", paciente.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Id", paciente.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Id", paciente.RacaId);
            return View(paciente);
        }

        // GET: Paciente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Especie)
                .Include(p => p.Proprietario)
                .Include(p => p.Raca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Paciente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}
