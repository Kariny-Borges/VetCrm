using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly VetCrmContext _context;

        public CategoriaController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias.ToListAsync();
            var categoriasViewModel = categorias.Select(c => new CategoriaViewModel
            {
                Id = c.Id,
                Nome = c.Nome
            }).ToList();

            return View(categoriasViewModel);
        }

        public IActionResult Create()
        {
            return View(new CategoriaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoriaViewModel categoriaViewModel)
        {
            if (ModelState.IsValid)
            {
                var categoria = new Categoria
                {
                    Nome = categoriaViewModel.Nome
                };

                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            var categoriaViewModel = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nome = categoria.Nome
            };

            return View(categoriaViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoriaViewModel categoriaViewModel)
        {
            if (ModelState.IsValid)
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null) return NotFound();

                categoria.Nome = categoriaViewModel.Nome;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            var categoriaViewModel = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nome = categoria.Nome
            };

            return View(categoriaViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null) _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}