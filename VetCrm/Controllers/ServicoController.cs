using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class ServicoController : Controller
    {
        private readonly VetCrmContext _context;

        public ServicoController(VetCrmContext context)
        {
            _context = context;
        }

        // Mostra a lista
        public async Task<IActionResult> Index()
        {
            var servicos = await _context.Servicos.ToListAsync();
            var servicosViewModel = servicos.Select(s => new ServicoViewModel
            {
                Id = s.Id,
                Nome = s.Nome,
                Preco = s.Preco
            }).ToList();

            return View(servicosViewModel);
        }

        // Mostra o formulário vazio
        public IActionResult Create()
        {
            return View();
        }

        // Recebe o formulário e salva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var servico = new Servico
                {
                    Nome = model.Nome,
                    Preco = model.Preco
                };

                _context.Servicos.Add(servico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Mostra o formulário já preenchido
        public async Task<IActionResult> Edit(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null) return NotFound();

            var servicoViewModel = new ServicoViewModel
            {
                Id = servico.Id,
                Nome = servico.Nome,
                Preco = servico.Preco
            };

            return View(servicoViewModel);
        }

        // Recebe o formulário e atualiza
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServicoViewModel servicoViewModel)
        {
            if (ModelState.IsValid)
            {
                var servico = await _context.Servicos.FindAsync(id);
                if (servico == null) return NotFound();

                servico.Nome = servicoViewModel.Nome;
                servico.Preco = servicoViewModel.Preco;
                _context.Servicos.Update(servico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(servicoViewModel);
        }

        // Mostra a tela de confirmação
        public async Task<IActionResult> Delete(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null)
            {
                return NotFound();
            }

            var servicoViewModel = new ServicoViewModel
            {
                Id = servico.Id,
                Nome = servico.Nome,
                Preco = servico.Preco
            };

            return View(servicoViewModel);
        }

        // Confirma e exclui
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico != null)
            {
                _context.Servicos.Remove(servico);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}