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
        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Estabelecimentos
                .Include(e => e.Endereco)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(e => e.Nome.Contains(busca) || e.CNPJ.Contains(busca));
            }

            ViewData["BuscaAtual"] = busca;
            return View(await query.ToListAsync());
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
            return View();
        }

        // POST: Estabelecimento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CNPJ")] Estabelecimento estabelecimento, Endereco endereco)
        {
            ModelState.Remove("Endereco");
            ModelState.Remove("UsuarioEstabelecimentos");
            ModelState.Remove("endereco.Id");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(endereco.Logradouro))
                {
                    _context.Enderecos.Add(endereco);
                    await _context.SaveChangesAsync();
                    estabelecimento.EnderecoId = endereco.Id;
                }

                _context.Add(estabelecimento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estabelecimento);
        }

        // GET: Estabelecimento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var estabelecimento = await _context.Estabelecimentos.Include(e => e.Endereco).FirstOrDefaultAsync(e => e.Id == id);
            if (estabelecimento == null) return NotFound();

            return View(estabelecimento);
        }

        // POST: Estabelecimento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CNPJ,EnderecoId")] Estabelecimento estabelecimento, Endereco endereco)
        {
            if (id != estabelecimento.Id) return NotFound();

            ModelState.Remove("Endereco");
            ModelState.Remove("UsuarioEstabelecimentos");
            ModelState.Remove("endereco.Id");

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(endereco.Logradouro))
                    {
                        if (estabelecimento.EnderecoId.HasValue && estabelecimento.EnderecoId.Value > 0)
                        {
                            var enderecoExistente = await _context.Enderecos.FindAsync(estabelecimento.EnderecoId.Value);
                            if (enderecoExistente != null)
                            {
                                enderecoExistente.CEP = endereco.CEP;
                                enderecoExistente.Logradouro = endereco.Logradouro;
                                enderecoExistente.Numero = endereco.Numero;
                                enderecoExistente.Complemento = endereco.Complemento;
                                enderecoExistente.Bairro = endereco.Bairro;
                                enderecoExistente.Cidade = endereco.Cidade;
                                enderecoExistente.Estado = endereco.Estado;
                            }
                        }
                        else
                        {
                            _context.Enderecos.Add(endereco);
                            await _context.SaveChangesAsync();
                            estabelecimento.EnderecoId = endereco.Id;
                        }
                    }

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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir este estabelecimento porque ele possui usuários vinculados.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EstabelecimentoExists(int id)
        {
            return _context.Estabelecimentos.Any(e => e.Id == id);
        }
    }
}