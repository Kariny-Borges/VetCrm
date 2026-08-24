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
    public class ConsultaController : Controller
    {
        private readonly VetCrmContext _context;

        public ConsultaController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Consulta
        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Veterinario)
                .Include(c => c.TipoConsulta)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(c => c.Paciente.Nome.Contains(busca) || c.Veterinario.Nome.Contains(busca) || c.Observacoes.Contains(busca));
            }

            var consultas = await query.ToListAsync();
            var consultasViewModel = consultas.Select(c => new ConsultaViewModel
            {
                Id = c.Id,
                DataConsulta = c.DataConsulta,
                Situacao = c.Situacao,
                Observacoes = c.Observacoes,
                TipoConsultaId = c.TipoConsultaId,
                PacienteId = c.PacienteId,
                VeterinarioId = c.VeterinarioId,
                TipoConsulta = c.TipoConsulta == null ? null : new TipoConsultaViewModel
                {
                    Id = c.TipoConsulta.Id,
                    Nome = c.TipoConsulta.Nome
                },
                Paciente = c.Paciente == null ? null : new PacienteViewModel
                {
                    Id = c.Paciente.Id,
                    Nome = c.Paciente.Nome
                },
                Veterinario = c.Veterinario == null ? null : new VeterinarioViewModel
                {
                    Id = c.Veterinario.Id,
                    Nome = c.Veterinario.Nome
                }
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(consultasViewModel);
        }

        // GET: Consulta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Veterinario)
                .Include(c => c.TipoConsulta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null)
            {
                return NotFound();
            }

            var consultaViewModel = new ConsultaViewModel
            {
                Id = consulta.Id,
                DataConsulta = consulta.DataConsulta,
                Situacao = consulta.Situacao,
                Observacoes = consulta.Observacoes,
                TipoConsultaId = consulta.TipoConsultaId,
                PacienteId = consulta.PacienteId,
                VeterinarioId = consulta.VeterinarioId,
                TipoConsulta = consulta.TipoConsulta == null ? null : new TipoConsultaViewModel
                {
                    Id = consulta.TipoConsulta.Id,
                    Nome = consulta.TipoConsulta.Nome
                },
                Paciente = consulta.Paciente == null ? null : new PacienteViewModel
                {
                    Id = consulta.Paciente.Id,
                    Nome = consulta.Paciente.Nome
                },
                Veterinario = consulta.Veterinario == null ? null : new VeterinarioViewModel
                {
                    Id = consulta.Veterinario.Id,
                    Nome = consulta.Veterinario.Nome
                }
            };

            return View(consultaViewModel);
        }

        // GET: Consulta/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome");
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Nome");
            ViewData["TipoConsultaId"] = new SelectList(_context.TiposConsulta, "Id", "Nome");
            return View(new ConsultaViewModel());
        }

        // POST: Consulta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsultaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var consulta = new Consulta
                {
                    DataConsulta = model.DataConsulta,
                    Observacoes = model.Observacoes,
                    TipoConsultaId = model.TipoConsultaId,
                    PacienteId = model.PacienteId,
                    VeterinarioId = model.VeterinarioId
                };
                _context.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", model.PacienteId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Nome", model.VeterinarioId);
            ViewData["TipoConsultaId"] = new SelectList(_context.TiposConsulta, "Id", "Nome", model.TipoConsultaId);
            return View(model);
        }

        // GET: Consulta/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null) return NotFound();

            var consultaViewModel = new ConsultaViewModel
            {
                Id = consulta.Id,
                DataConsulta = consulta.DataConsulta,
                Observacoes = consulta.Observacoes,
                TipoConsultaId = consulta.TipoConsultaId,
                PacienteId = consulta.PacienteId,
                VeterinarioId = consulta.VeterinarioId
            };

            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", consultaViewModel.PacienteId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Nome", consultaViewModel.VeterinarioId);
            ViewData["TipoConsultaId"] = new SelectList(_context.TiposConsulta, "Id", "Nome", consultaViewModel.TipoConsultaId);
            return View(consultaViewModel);
        }

        // POST: Consulta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConsultaViewModel consultaViewModel)
        {
            if (ModelState.IsValid)
            {
                var consulta = await _context.Consultas.FindAsync(id);
                if (consulta == null) return NotFound();

                consulta.DataConsulta = consultaViewModel.DataConsulta;
                consulta.Observacoes = consultaViewModel.Observacoes;
                consulta.TipoConsultaId = consultaViewModel.TipoConsultaId;
                consulta.PacienteId = consultaViewModel.PacienteId;
                consulta.VeterinarioId = consultaViewModel.VeterinarioId;
                _context.Consultas.Update(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", consultaViewModel.PacienteId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Nome", consultaViewModel.VeterinarioId);
            ViewData["TipoConsultaId"] = new SelectList(_context.TiposConsulta, "Id", "Nome", consultaViewModel.TipoConsultaId);
            return View(consultaViewModel);
        }

        // POST: Consulta/MudarSituacao
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MudarSituacao(int id, SituacaoConsulta situacao)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            consulta.Situacao = situacao;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Consulta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Veterinario)
                .Include(c => c.TipoConsulta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null)
            {
                return NotFound();
            }

            var consultaViewModel = new ConsultaViewModel
            {
                Id = consulta.Id,
                DataConsulta = consulta.DataConsulta,
                Situacao = consulta.Situacao,
                Observacoes = consulta.Observacoes,
                TipoConsultaId = consulta.TipoConsultaId,
                PacienteId = consulta.PacienteId,
                VeterinarioId = consulta.VeterinarioId,
                TipoConsulta = consulta.TipoConsulta == null ? null : new TipoConsultaViewModel
                {
                    Id = consulta.TipoConsulta.Id,
                    Nome = consulta.TipoConsulta.Nome
                },
                Paciente = consulta.Paciente == null ? null : new PacienteViewModel
                {
                    Id = consulta.Paciente.Id,
                    Nome = consulta.Paciente.Nome
                },
                Veterinario = consulta.Veterinario == null ? null : new VeterinarioViewModel
                {
                    Id = consulta.Veterinario.Id,
                    Nome = consulta.Veterinario.Nome
                }
            };

            return View(consultaViewModel);
        }

        // POST: Consulta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir esta consulta porque ela possui prontuário vinculado.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ConsultaExists(int id)
        {
            return _context.Consultas.Any(e => e.Id == id);
        }
    }
}