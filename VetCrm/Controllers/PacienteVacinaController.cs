using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

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
            CarregarDropdowns();
            return View();
        }

        // POST: PacienteVacina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PacienteVacinaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pacienteVacina = new PacienteVacina
                {
                    PacienteId = model.PacienteId,
                    VacinaId = model.VacinaId,
                    DataAplicacao = model.DataAplicacao,
                    DataProximaDose = model.DataProximaDose
                    // ProntuarioId fica null: vacinação lançada fora de um atendimento.
                };

                _context.Add(pacienteVacina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CarregarDropdowns(model.PacienteId, model.VacinaId);
            return View(model);
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

            var model = new PacienteVacinaViewModel
            {
                Id = pacienteVacina.Id,
                PacienteId = pacienteVacina.PacienteId,
                VacinaId = pacienteVacina.VacinaId,
                DataAplicacao = pacienteVacina.DataAplicacao,
                DataProximaDose = pacienteVacina.DataProximaDose
            };

            CarregarDropdowns(model.PacienteId, model.VacinaId);
            return View(model);
        }

        // POST: PacienteVacina/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PacienteVacinaViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pacienteVacina = await _context.PacienteVacinas.FindAsync(id);
                    if (pacienteVacina == null)
                    {
                        return NotFound();
                    }

                    // Só o que a tela edita. ProntuarioId continua com o valor do banco.
                    pacienteVacina.PacienteId = model.PacienteId;
                    pacienteVacina.VacinaId = model.VacinaId;
                    pacienteVacina.DataAplicacao = model.DataAplicacao;
                    pacienteVacina.DataProximaDose = model.DataProximaDose;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteVacinaExists(model.Id))
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

            CarregarDropdowns(model.PacienteId, model.VacinaId);
            return View(model);
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

        // Monta os dois dropdowns da tela, mostrando o NOME (antes mostrava o Id).
        private void CarregarDropdowns(int? pacienteId = null, int? vacinaId = null)
        {
            ViewData["PacienteId"] = new SelectList(
                _context.Pacientes.OrderBy(p => p.Nome), "Id", "Nome", pacienteId);
            ViewData["VacinaId"] = new SelectList(
                _context.Vacinas.OrderBy(v => v.Nome), "Id", "Nome", vacinaId);
        }

        private bool PacienteVacinaExists(int id)
        {
            return _context.PacienteVacinas.Any(e => e.Id == id);
        }
    }
}