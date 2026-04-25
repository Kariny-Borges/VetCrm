using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;

namespace VetCrm.Controllers
{
    public class EstabelecimentoController : Controller
    {
        private readonly VetCrmContext _context;

        public EstabelecimentoController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Estabelecimento
        public async Task<IActionResult> Index()
        {
            var estabelecimentos = _context.Estabelecimentos
                .Include(e => e.Endereco);
            return View(await estabelecimentos.ToListAsync());
        }

        // GET: Estabelecimento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var estabelecimento = await _context.Estabelecimentos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (estabelecimento == null) return NotFound();

            return View(estabelecimento);
        }

        // GET: Estabelecimento/Create
        public IActionResult Create()
        {
            ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "Id", "Logradouro");
            return View();
        }

        // POST: Estabelecimento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CNPJ,EnderecoId")] Estabelecimento estabelecimento)
        {
            ModelState.Remove("Endereco");
            ModelState.Remove("UsuarioEstabelecimentos");

            if (ModelState.IsValid)
            {
                _context.Add(estabelecimento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "Id", "Logradouro", estabelecimento.EnderecoId);
            return View(estabelecimento);
        }

        // GET: Estabelecimento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var estabelecimento = await _context.Estabelecimentos.FindAsync(id);
            if (estabelecimento == null) return NotFound();

            ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "Id", "Logradouro", estabelecimento.EnderecoId);
            return View(estabelecimento);
        }

        // POST: Estabelecimento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CNPJ,EnderecoId")] Estabelecimento estabelecimento)
        {
            if (id != estabelecimento.Id) return NotFound();

            ModelState.Remove("Endereco");
            ModelState.Remove("UsuarioEstabelecimentos");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estabelecimento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstabelecimentoExists(estabelecimento.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "Id", "Logradouro", estabelecimento.EnderecoId);
            return View(estabelecimento);
        }

        // GET: Estabelecimento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var estabelecimento = await _context.Estabelecimentos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (estabelecimento == null) return NotFound();

            return View(estabelecimento);
        }

        // POST: Estabelecimento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estabelecimento = await _context.Estabelecimentos.FindAsync(id);
            if (estabelecimento != null) _context.Estabelecimentos.Remove(estabelecimento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstabelecimentoExists(int id)
        {
            return _context.Estabelecimentos.Any(e => e.Id == id);
        }
    }
}