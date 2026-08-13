using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class FormaPagamentoController : Controller
    {
        private readonly VetCrmContext _context;

        public FormaPagamentoController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var formasPagamento = await _context.FormasPagamento.ToListAsync();
            var formasPagamentoViewModel = formasPagamento.Select(f => new FormaPagamentoViewModel
            {
                Id = f.Id,
                Nome = f.Nome
            }).ToList();

            return View(formasPagamentoViewModel);
        }

        public IActionResult Create()
        {
            return View(new FormaPagamentoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(FormaPagamentoViewModel formaPagamentoViewModel)
        {
            if (ModelState.IsValid)
            {
                var formaPagamento = new FormaPagamento
                {
                    Nome = formaPagamentoViewModel.Nome
                };

                _context.Add(formaPagamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(formaPagamentoViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var formaPagamento = await _context.FormasPagamento.FindAsync(id);
            if (formaPagamento == null) return NotFound();

            var formaPagamentoViewModel = new FormaPagamentoViewModel
            {
                Id = formaPagamento.Id,
                Nome = formaPagamento.Nome
            };

            return View(formaPagamentoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FormaPagamentoViewModel formaPagamentoViewModel)
        {
            if (ModelState.IsValid)
            {
                var formaPagamento = await _context.FormasPagamento.FindAsync(id);
                if (formaPagamento == null) return NotFound();

                formaPagamento.Nome = formaPagamentoViewModel.Nome;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(formaPagamentoViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var formaPagamento = await _context.FormasPagamento.FindAsync(id);
            if (formaPagamento == null) return NotFound();

            var formaPagamentoViewModel = new FormaPagamentoViewModel
            {
                Id = formaPagamento.Id,
                Nome = formaPagamento.Nome
            };

            return View(formaPagamentoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formaPagamento = await _context.FormasPagamento.FindAsync(id);
            if (formaPagamento != null) _context.FormasPagamento.Remove(formaPagamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}