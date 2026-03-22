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
    public class ProntuarioController : Controller
    {
        private readonly VetCrmContext _context;

        public ProntuarioController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Prontuario
        public async Task<IActionResult> Index()
        {
            var vetCrmContext = _context.Prontuarios.Include(p => p.Consulta).Include(p => p.Paciente);
            return View(await vetCrmContext.ToListAsync());
        }

        // GET: Prontuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prontuario = await _context.Prontuarios
                .Include(p => p.Consulta)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prontuario == null)
            {
                return NotFound();
            }

            return View(prontuario);
        }

        // GET: Prontuario/Create
        public IActionResult Create()
        {
            ViewData["ConsultaId"] = new SelectList(_context.Consultas, "Id", "Id");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id");
            return View();
        }

        // POST: Prontuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Diagnostico,Tratamento,DataRegistro,PacienteId,ConsultaId")] Prontuario prontuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prontuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsultaId"] = new SelectList(_context.Consultas, "Id", "Id", prontuario.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id", prontuario.PacienteId);
            return View(prontuario);
        }

        // GET: Prontuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prontuario = await _context.Prontuarios.FindAsync(id);
            if (prontuario == null)
            {
                return NotFound();
            }
            ViewData["ConsultaId"] = new SelectList(_context.Consultas, "Id", "Id", prontuario.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id", prontuario.PacienteId);
            return View(prontuario);
        }

        // POST: Prontuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Diagnostico,Tratamento,DataRegistro,PacienteId,ConsultaId")] Prontuario prontuario)
        {
            if (id != prontuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prontuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProntuarioExists(prontuario.Id))
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
            ViewData["ConsultaId"] = new SelectList(_context.Consultas, "Id", "Id", prontuario.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id", prontuario.PacienteId);
            return View(prontuario);
        }

        // GET: Prontuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prontuario = await _context.Prontuarios
                .Include(p => p.Consulta)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prontuario == null)
            {
                return NotFound();
            }

            return View(prontuario);
        }

        // POST: Prontuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prontuario = await _context.Prontuarios.FindAsync(id);
            if (prontuario != null)
            {
                _context.Prontuarios.Remove(prontuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProntuarioExists(int id)
        {
            return _context.Prontuarios.Any(e => e.Id == id);
        }
    }
}
