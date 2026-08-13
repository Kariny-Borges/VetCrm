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
    public class EspeciesController : Controller
    {
        private readonly VetCrmContext _context;

        public EspeciesController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Especies
        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Especies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(e => e.Nome.Contains(busca));
            }

            var especies = await query.ToListAsync();
            var especiesViewModel = especies.Select(e => new EspecieViewModel
            {
                Id = e.Id,
                Nome = e.Nome
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(especiesViewModel);
        }

        // GET: Especies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (especie == null)
            {
                return NotFound();
            }

            var especieViewModel = new EspecieViewModel
            {
                Id = especie.Id,
                Nome = especie.Nome
            };

            return View(especieViewModel);
        }

        // GET: Especies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EspecieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var especie = new Especie
                {
                    Nome = model.Nome
                };
                _context.Add(especie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Especies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies.FindAsync(id);
            if (especie == null)
            {
                return NotFound();
            }

            var model = new EspecieViewModel
            {
                Id = especie.Id,
                Nome = especie.Nome
            };
            return View(model);
        }

        // POST: Especies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EspecieViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var especie = await _context.Especies.FindAsync(id);
                    if (especie == null)
                    {
                        return NotFound();
                    }

                    especie.Nome = model.Nome;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecieExists(model.Id))
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

        // GET: Especies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (especie == null)
            {
                return NotFound();
            }

            var especieViewModel = new EspecieViewModel
            {
                Id = especie.Id,
                Nome = especie.Nome
            };

            return View(especieViewModel);
        }

        // POST: Especies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie != null)
            {
                _context.Especies.Remove(especie);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir esta espécie porque existem raças ou pacientes vinculados a ela.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EspecieExists(int id)
        {
            return _context.Especies.Any(e => e.Id == id);
        }
    }
}