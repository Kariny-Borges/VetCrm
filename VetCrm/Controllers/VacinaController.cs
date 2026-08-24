using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class VacinaController : Controller
    {
        private readonly VetCrmContext _context;

        public VacinaController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Vacinas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(v => v.Nome.Contains(busca) || v.Descricao.Contains(busca));
            }

            var vacinas = await query.ToListAsync();
            var vacinasViewModel = vacinas.Select(v => new VacinaViewModel
            {
                Id = v.Id,
                Nome = v.Nome,
                Descricao = v.Descricao,
                Lote = v.Lote,
                Validade = v.Validade,
                Fabricante = v.Fabricante
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(vacinasViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VacinaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vacina = new Vacina
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Lote = model.Lote,
                    Validade = model.Validade,
                    Fabricante = model.Fabricante
                };

                _context.Add(vacina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vacina = await _context.Vacinas.FindAsync(id);
            if (vacina == null) return NotFound();

            var vacinaViewModel = new VacinaViewModel
            {
                Id = vacina.Id,
                Nome = vacina.Nome,
                Descricao = vacina.Descricao,
                Lote = vacina.Lote,
                Validade = vacina.Validade,
                Fabricante = vacina.Fabricante
            };

            return View(vacinaViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, VacinaViewModel vacinaViewModel)
        {
            if (ModelState.IsValid)
            {
                var vacina = await _context.Vacinas.FindAsync(id);
                if (vacina == null) return NotFound();

                vacina.Nome = vacinaViewModel.Nome;
                vacina.Descricao = vacinaViewModel.Descricao;
                vacina.Lote = vacinaViewModel.Lote;
                vacina.Validade = vacinaViewModel.Validade;
                vacina.Fabricante = vacinaViewModel.Fabricante;
                _context.Vacinas.Update(vacina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vacinaViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vacina = await _context.Vacinas.FindAsync(id);
            if (vacina == null) return NotFound();

            var vacinaViewModel = new VacinaViewModel
            {
                Id = vacina.Id,
                Nome = vacina.Nome,
                Descricao = vacina.Descricao,
                Lote = vacina.Lote,
                Validade = vacina.Validade,
                Fabricante = vacina.Fabricante
            };

            return View(vacinaViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacina = await _context.Vacinas.FindAsync(id);
            if (vacina != null) _context.Vacinas.Remove(vacina);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir esta vacina porque existem pacientes vacinados com ela.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var vacina = await _context.Vacinas.FindAsync(id);
            if (vacina == null) return NotFound();

            var vacinaViewModel = new VacinaViewModel
            {
                Id = vacina.Id,
                Nome = vacina.Nome,
                Descricao = vacina.Descricao,
                Lote = vacina.Lote,
                Validade = vacina.Validade,
                Fabricante = vacina.Fabricante
            };

            return View(vacinaViewModel);
        }
    }
}