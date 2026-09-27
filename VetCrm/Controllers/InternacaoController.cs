using Microsoft.AspNetCore.Mvc;
using VetCrm.Data;
using VetCrm.ViewModels;
using Microsoft.EntityFrameworkCore;
using VetCrm.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace VetCrm.Controllers
{
    public class InternacaoController : Controller
    {
        private VetCrmContext _context;

        public InternacaoController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var internacoes = await _context.Internacoes.Include(i => i.Paciente).ToListAsync();
            var internacoesViewModel = internacoes.Select(i => new InternacaoViewModel

            {
                Id = i.Id,
                Motivo = i.Motivo,
                DataEntrada = i.DataEntrada,
                DataSaida = i.DataSaida,
                PacienteId = i.PacienteId,
                Paciente = i.Paciente == null ? null : new PacienteViewModel
                {
                    Id = i.Paciente.Id,
                    Nome = i.Paciente.Nome
                }
            }).ToList();
            return View(internacoesViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internacao = await _context.Internacoes
                .Include(i => i.Paciente)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (internacao == null)
            {
                return NotFound();
            }

            var internacaoViewModel = new InternacaoViewModel
            {
                Id = internacao.Id,
                Motivo = internacao.Motivo,
                DataEntrada = internacao.DataEntrada,
                DataSaida = internacao.DataSaida,
                PacienteId = internacao.PacienteId,
                Paciente = internacao.Paciente == null ? null : new PacienteViewModel
                {
                    Id = internacao.Paciente.Id,
                    Nome = internacao.Paciente.Nome
                }
            };

            return View(internacaoViewModel);
        }

        // GET: Internacao/Create
        public IActionResult Create()
        {
            CarregarDropdown();
            return View();
        }

        // POST: Internacao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InternacaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var internacao = new Internacao
                {
                    PacienteId = model.PacienteId,
                    Motivo = model.Motivo,
                    DataEntrada = model.DataEntrada,
                    DataSaida = model.DataSaida
                };

                _context.Add(internacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CarregarDropdown(model.PacienteId);
            return View(model);
        }
        // GET: Internacao/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var internacao = await _context.Internacoes.FindAsync(id);
            if (internacao == null)
            {
                return NotFound();
            }

            var internacaoViewModel = new InternacaoViewModel
            {
                Id = internacao.Id,
                PacienteId = internacao.PacienteId,
                Motivo = internacao.Motivo,
                DataEntrada = internacao.DataEntrada,
                DataSaida = internacao.DataSaida
            };

            CarregarDropdown(internacaoViewModel.PacienteId);
            return View(internacaoViewModel);
        }

        // POST: Internacao/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InternacaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var internacao = await _context.Internacoes.FindAsync(id);
                if (internacao == null)
                {
                    return NotFound();
                }

                internacao.PacienteId = model.PacienteId;
                internacao.Motivo = model.Motivo;
                internacao.DataEntrada = model.DataEntrada;
                internacao.DataSaida = model.DataSaida;
                _context.Internacoes.Update(internacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CarregarDropdown(model.PacienteId);
            return View(model);
        }

        // GET: Internacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internacao = await _context.Internacoes
                .Include(i => i.Paciente)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (internacao == null)
            {
                return NotFound();
            }

            var internacaoViewModel = new InternacaoViewModel
            {
                Id = internacao.Id,
                Motivo = internacao.Motivo,
                DataEntrada = internacao.DataEntrada,
                DataSaida = internacao.DataSaida,
                PacienteId = internacao.PacienteId,
                Paciente = internacao.Paciente == null ? null : new PacienteViewModel
                {
                    Id = internacao.Paciente.Id,
                    Nome = internacao.Paciente.Nome
                }
            };

            return View(internacaoViewModel);
        }

        // POST: Internacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var internacao = await _context.Internacoes.FindAsync(id);
            if (internacao != null)
            {
                _context.Internacoes.Remove(internacao);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Monta o dropdown de Pacientes, mostrando o Nome (o valor salvo é o Id).
        private void CarregarDropdown(int? pacienteId = null)
        {
            ViewData["PacienteId"] = new SelectList(
                _context.Pacientes.OrderBy(p => p.Nome), "Id", "Nome", pacienteId);
        }
    }
}
