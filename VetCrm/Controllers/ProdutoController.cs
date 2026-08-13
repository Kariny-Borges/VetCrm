using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly VetCrmContext _context;

        public ProdutoController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Produtos.Include(p => p.Categoria).AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(p => p.Nome.Contains(busca) || p.Categoria.Nome.Contains(busca));
            }

            var produtos = await query.ToListAsync();
            var produtosViewModel = produtos.Select(p => new ProdutoViewModel
            {
                Id = p.Id,
                Nome = p.Nome,
                CategoriaId = p.CategoriaId,
                Quantidade = p.Quantidade,
                Preco = p.Preco,
                Categoria = p.Categoria == null ? null : new CategoriaViewModel
                {
                    Id = p.Categoria.Id,
                    Nome = p.Categoria.Nome
                }
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(produtosViewModel);
        }

        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nome");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProdutoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var produto = new Produto
                {
                    Nome = model.Nome,
                    CategoriaId = model.CategoriaId,
                    Quantidade = model.Quantidade,
                    Preco = model.Preco
                };

                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nome", model.CategoriaId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            var model = new ProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                CategoriaId = produto.CategoriaId,
                Quantidade = produto.Quantidade,
                Preco = produto.Preco
            };

            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nome", model.CategoriaId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProdutoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var produto = await _context.Produtos.FindAsync(model.Id);
                if (produto == null) return NotFound();

                produto.Nome = model.Nome;
                produto.CategoriaId = model.CategoriaId;
                produto.Quantidade = model.Quantidade;
                produto.Preco = model.Preco;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nome", model.CategoriaId);
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null) return NotFound();

            var produtoViewModel = new ProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                CategoriaId = produto.CategoriaId,
                Quantidade = produto.Quantidade,
                Preco = produto.Preco,
                Categoria = produto.Categoria == null ? null : new CategoriaViewModel
                {
                    Id = produto.Categoria.Id,
                    Nome = produto.Categoria.Nome
                }
            };

            return View(produtoViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null) return NotFound();

            var produtoViewModel = new ProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                CategoriaId = produto.CategoriaId,
                Quantidade = produto.Quantidade,
                Preco = produto.Preco,
                Categoria = produto.Categoria == null ? null : new CategoriaViewModel
                {
                    Id = produto.Categoria.Id,
                    Nome = produto.Categoria.Nome
                }
            };

            return View(produtoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null) _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}