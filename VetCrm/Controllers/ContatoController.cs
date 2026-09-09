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
    public class ContatoController : Controller
    {
        private readonly VetCrmContext _context;

        public ContatoController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Contato
        public async Task<IActionResult> Index(int pagina = 1)
        {
            var query = _context.Contato.AsQueryable();

            var itensPorPagina = 10;
            var totalItens = await query.CountAsync();

            var contatos = await query
                .OrderBy(c => c.Id)
                .Skip((pagina - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToListAsync();
            var contatosViewModel = contatos.Select(c => new ContatoViewModel
            {
                Id = c.Id,
                Tipo = c.Tipo,
                Valor = c.Valor
            }).ToList();

            return View(new ListaPaginadaViewModel<ContatoViewModel>
            {
                Itens = contatosViewModel,
                PaginaAtual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalItens / (double)itensPorPagina)
            });
        }

        // GET: Contato/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contato == null)
            {
                return NotFound();
            }

            var contatoViewModel = new ContatoViewModel
            {
                Id = contato.Id,
                Tipo = contato.Tipo,
                Valor = contato.Valor
            };

            return View(contatoViewModel);
        }

        // GET: Contato/Create
        public IActionResult Create()
        {
            return View(new ContatoViewModel());
        }

        // POST: Contato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContatoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contato = new Contato
                {
                    Tipo = model.Tipo,
                    Valor = model.Valor
                };
                _context.Add(contato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Contato/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contato = await _context.Contato.FindAsync(id);
            if (contato == null) return NotFound();

            var contatoViewModel = new ContatoViewModel
            {
                Id = contato.Id,
                Tipo = contato.Tipo,
                Valor = contato.Valor
            };

            return View(contatoViewModel);
        }

        // POST: Contato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContatoViewModel contatoViewModel)
        {
            if (ModelState.IsValid)
            {
                var contato = await _context.Contato.FindAsync(id);
                if (contato == null) return NotFound();

                contato.Tipo = contatoViewModel.Tipo;
                contato.Valor = contatoViewModel.Valor;
                _context.Contato.Update(contato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contatoViewModel);
        }

        // GET: Contato/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contato == null)
            {
                return NotFound();
            }

            var contatoViewModel = new ContatoViewModel
            {
                Id = contato.Id,
                Tipo = contato.Tipo,
                Valor = contato.Valor
            };

            return View(contatoViewModel);
        }

        // POST: Contato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contato = await _context.Contato.FindAsync(id);
            if (contato != null)
            {
                _context.Contato.Remove(contato);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir este contato porque ele possui registros vinculados.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContatoExists(int id)
        {
            return _context.Contato.Any(e => e.Id == id);
        }
    }
}