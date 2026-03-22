using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;

namespace VetCrm.Controllers
{
    public class EspecieController : Controller
    {
        private readonly VetCrmContext _context;

        public EspecieController(VetCrmContext context)
        {
            _context = context;
        }

        // Lista todas as espécies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Especies.ToListAsync());
        }

        // Tela de criar novo
        public IActionResult Create()
        {
            return View();
        }

        // Salva novo no banco
        [HttpPost]
        public async Task<IActionResult> Create(Especie especie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(especie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(especie);
        }

        // Tela de editar
        public async Task<IActionResult> Edit(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie == null) return NotFound();
            return View(especie);
        }

        // Salva edição
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Especie especie)
        {
            if (ModelState.IsValid)
            {
                _context.Update(especie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(especie);
        }

        // Tela de deletar
        public async Task<IActionResult> Delete(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie == null) return NotFound();
            return View(especie);
        }

        // Confirma delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie != null) _context.Especies.Remove(especie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Detalhes
        public async Task<IActionResult> Details(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie == null) return NotFound();
            return View(especie);
        }
    }
}