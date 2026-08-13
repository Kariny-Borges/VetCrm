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
            var servico = new Servico
            {
                Nome = model.Nome,
                Preco = model.Preco
            };

            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Mostra o formulário já preenchido
        public async Task<IActionResult> Edit(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null)
            {
                return NotFound();
            }

            var model = new ServicoViewModel
            {
                Id = servico.Id,
                Nome = servico.Nome,
                Preco = servico.Preco
            };

            return View(model);
        }

        // Recebe o formulário e atualiza
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServicoViewModel model)
        {
            var servico = await _context.Servicos.FindAsync(model.Id);
            if (servico == null)
            {
                return NotFound();
            }

            servico.Nome = model.Nome;
            servico.Preco = model.Preco;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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