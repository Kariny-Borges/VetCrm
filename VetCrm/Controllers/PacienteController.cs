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

        public async Task<IActionResult> Index(string busca, int pagina = 1)
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

            var itensPorPagina = 10;
            var totalItens = await query.CountAsync();

            var pacientes = await query
                .OrderBy(p => p.Id)
                .Skip((pagina - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToListAsync();
            var pacientesViewModel = pacientes.Select(p => new PacienteViewModel
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade,
                Sexo = p.Sexo,
                Peso = p.Peso,
                DataCadastro = p.DataCadastro,
                ProprietarioId = p.ProprietarioId,
                EspecieId = p.EspecieId,
                RacaId = p.RacaId,
                Proprietario = p.Proprietario == null ? null : new ProprietarioViewModel
                {
                    Id = p.Proprietario.Id,
                    Nome = p.Proprietario.Nome
                },
                Especie = p.Especie == null ? null : new EspecieViewModel
                {
                    Id = p.Especie.Id,
                    Nome = p.Especie.Nome
                },
                Raca = p.Raca == null ? null : new RacaViewModel
                {
                    Id = p.Raca.Id,
                    Nome = p.Raca.Nome
                }
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(new ListaPaginadaViewModel<PacienteViewModel>
            {
                Itens = pacientesViewModel,
                PaginaAtual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalItens / (double)itensPorPagina),
                Busca = busca
            });
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

            var pacienteViewModel = new PacienteViewModel
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Idade = paciente.Idade,
                Sexo = paciente.Sexo,
                Peso = paciente.Peso,
                DataCadastro = paciente.DataCadastro,
                ProprietarioId = paciente.ProprietarioId,
                EspecieId = paciente.EspecieId,
                RacaId = paciente.RacaId,
                Proprietario = paciente.Proprietario == null ? null : new ProprietarioViewModel
                {
                    Id = paciente.Proprietario.Id,
                    Nome = paciente.Proprietario.Nome
                },
                Especie = paciente.Especie == null ? null : new EspecieViewModel
                {
                    Id = paciente.Especie.Id,
                    Nome = paciente.Especie.Nome
                },
                Raca = paciente.Raca == null ? null : new RacaViewModel
                {
                    Id = paciente.Raca.Id,
                    Nome = paciente.Raca.Nome
                }
            };

            return View(pacienteViewModel);
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
                    DataCadastro = DateTime.Now,
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

        public async Task<IActionResult> Edit(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            var pacienteViewModel = new PacienteViewModel
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Idade = paciente.Idade,
                Sexo = paciente.Sexo,
                Peso = paciente.Peso,
                ProprietarioId = paciente.ProprietarioId,
                EspecieId = paciente.EspecieId,
                RacaId = paciente.RacaId
            };

            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", pacienteViewModel.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome", pacienteViewModel.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome", pacienteViewModel.RacaId);
            return View(pacienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PacienteViewModel pacienteViewModel)
        {
            if (ModelState.IsValid)
            {
                var paciente = await _context.Pacientes.FindAsync(id);
                if (paciente == null) return NotFound();

                paciente.Nome = pacienteViewModel.Nome;
                paciente.Idade = pacienteViewModel.Idade;
                paciente.Sexo = pacienteViewModel.Sexo;
                paciente.Peso = pacienteViewModel.Peso;
                paciente.ProprietarioId = pacienteViewModel.ProprietarioId;
                paciente.EspecieId = pacienteViewModel.EspecieId;
                paciente.RacaId = pacienteViewModel.RacaId;
                _context.Pacientes.Update(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecieId"] = new SelectList(_context.Especies, "Id", "Nome", pacienteViewModel.EspecieId);
            ViewData["ProprietarioId"] = new SelectList(_context.Proprietarios, "Id", "Nome", pacienteViewModel.ProprietarioId);
            ViewData["RacaId"] = new SelectList(_context.Racas, "Id", "Nome", pacienteViewModel.RacaId);
            return View(pacienteViewModel);
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

            var pacienteViewModel = new PacienteViewModel
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Idade = paciente.Idade,
                Sexo = paciente.Sexo,
                Peso = paciente.Peso,
                DataCadastro = paciente.DataCadastro,
                ProprietarioId = paciente.ProprietarioId,
                EspecieId = paciente.EspecieId,
                RacaId = paciente.RacaId,
                Proprietario = paciente.Proprietario == null ? null : new ProprietarioViewModel
                {
                    Id = paciente.Proprietario.Id,
                    Nome = paciente.Proprietario.Nome
                },
                Especie = paciente.Especie == null ? null : new EspecieViewModel
                {
                    Id = paciente.Especie.Id,
                    Nome = paciente.Especie.Nome
                },
                Raca = paciente.Raca == null ? null : new RacaViewModel
                {
                    Id = paciente.Raca.Id,
                    Nome = paciente.Raca.Nome
                }
            };

            return View(pacienteViewModel);
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