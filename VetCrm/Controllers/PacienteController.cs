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
    public class PacienteController : Controller
    {
        private readonly VetCrmContext _context;

        public PacienteController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Pacientes
                .Include(p => p.Especie)
                .Include(p => p.Proprietario)
                .Include(p => p.Raca)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(p => p.Nome.Contains(busca) || p.Proprietario.Nome.Contains(busca));
            }

            ViewData["BuscaAtual"] = busca;
            return View(await query.ToListAsync());
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
        public async Task<IActionResult> Create(PacienteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var paciente = new Paciente
                {
                    Nome = model.Nome,
                    Idade = model.Idade,
                    Sexo = model.Sexo,
                    Peso = model.Peso,
                    DataCadastro = model.DataCadastro,
                    ProprietarioId = model.ProprietarioId,
                    EspecieId = model.EspecieId,
                    RacaId = model.RacaId
                };
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", model.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome", model.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome", model.RacaId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            var model = new PacienteViewModel
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Idade = paciente.Idade,
                Sexo = paciente.Sexo,
                Peso = paciente.Peso,
                DataCadastro = paciente.DataCadastro,
                ProprietarioId = paciente.ProprietarioId,
                EspecieId = paciente.EspecieId,
                RacaId = paciente.RacaId
            };

            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", model.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome", model.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome", model.RacaId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PacienteViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var paciente = await _context.Pacientes.FindAsync(id);
                    if (paciente == null) return NotFound();

                    paciente.Nome = model.Nome;
                    paciente.Idade = model.Idade;
                    paciente.Sexo = model.Sexo;
                    paciente.Peso = model.Peso;
                    paciente.DataCadastro = model.DataCadastro;
                    paciente.ProprietarioId = model.ProprietarioId;
                    paciente.EspecieId = model.EspecieId;
                    paciente.RacaId = model.RacaId;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", model.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome", model.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome", model.RacaId);
            return View(model);
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir este paciente porque ele possui consultas, prontuários ou vacinas vinculadas.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}