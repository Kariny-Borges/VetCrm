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
        public async Task<IActionResult> Index()
        {
            var contatos = await _context.Contato.ToListAsync();
            var contatosViewModel = contatos.Select(c => new ContatoViewModel
            {
                Id = c.Id,
                Tipo = c.Tipo,
                Valor = c.Valor
            }).ToList();

            return View(contatosViewModel);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato.FindAsync(id);
            if (contato == null)
            {
                return NotFound();
            }

            var model = new ContatoViewModel
            {
                Id = contato.Id,
                Tipo = contato.Tipo,
                Valor = contato.Valor
            };
            return View(model);
        }

        // POST: Contato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContatoViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contato = await _context.Contato.FindAsync(id);
                    if (contato == null)
                    {
                        return NotFound();
                    }

                    contato.Tipo = model.Tipo;
                    contato.Valor = model.Valor;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContatoExists(model.Id))
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
            return View(model);
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