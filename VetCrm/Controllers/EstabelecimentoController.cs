using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

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

            var estabelecimentos = await query.ToListAsync();
            var estabelecimentosViewModel = estabelecimentos.Select(e => new EstabelecimentoViewModel
            {
                Id = e.Id,
                Nome = e.Nome,
                CNPJ = e.CNPJ,
                EnderecoId = e.EnderecoId,
                Endereco = e.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = e.Endereco.Id,
                    CEP = e.Endereco.CEP,
                    Logradouro = e.Endereco.Logradouro,
                    Numero = e.Endereco.Numero,
                    Complemento = e.Endereco.Complemento,
                    Bairro = e.Endereco.Bairro,
                    Cidade = e.Endereco.Cidade,
                    Estado = e.Endereco.Estado
                }
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(estabelecimentosViewModel);
        }

        // GET: Estabelecimento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var estabelecimento = await _context.Estabelecimentos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (estabelecimento == null) return NotFound();

            var estabelecimentoViewModel = new EstabelecimentoViewModel
            {
                Id = estabelecimento.Id,
                Nome = estabelecimento.Nome,
                CNPJ = estabelecimento.CNPJ,
                EnderecoId = estabelecimento.EnderecoId,
                Endereco = estabelecimento.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = estabelecimento.Endereco.Id,
                    CEP = estabelecimento.Endereco.CEP,
                    Logradouro = estabelecimento.Endereco.Logradouro,
                    Numero = estabelecimento.Endereco.Numero,
                    Complemento = estabelecimento.Endereco.Complemento,
                    Bairro = estabelecimento.Endereco.Bairro,
                    Cidade = estabelecimento.Endereco.Cidade,
                    Estado = estabelecimento.Endereco.Estado
                }
            };

            return View(estabelecimentoViewModel);
        }

        // GET: Estabelecimento/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estabelecimento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstabelecimentoViewModel model, EnderecoViewModel endereco)
        {
            if (ModelState.IsValid)
            {
                var estabelecimento = new Estabelecimento
                {
                    Nome = model.Nome,
                    CNPJ = model.CNPJ
                };

                if (!string.IsNullOrWhiteSpace(endereco.Logradouro))
                {
                    var novoEndereco = new Endereco
                    {
                        CEP = endereco.CEP,
                        Logradouro = endereco.Logradouro,
                        Numero = endereco.Numero,
                        Complemento = endereco.Complemento,
                        Bairro = endereco.Bairro,
                        Cidade = endereco.Cidade,
                        Estado = endereco.Estado
                    };
                    _context.Enderecos.Add(novoEndereco);
                    await _context.SaveChangesAsync();
                    estabelecimento.EnderecoId = novoEndereco.Id;
                }

                _context.Add(estabelecimento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Estabelecimento/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var estabelecimento = await _context.Estabelecimentos.Include(e => e.Endereco).FirstOrDefaultAsync(e => e.Id == id);
            if (estabelecimento == null) return NotFound();

            var estabelecimentoViewModel = new EstabelecimentoViewModel
            {
                Id = estabelecimento.Id,
                Nome = estabelecimento.Nome,
                CNPJ = estabelecimento.CNPJ,
                EnderecoId = estabelecimento.EnderecoId,
                Endereco = estabelecimento.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = estabelecimento.Endereco.Id,
                    CEP = estabelecimento.Endereco.CEP,
                    Logradouro = estabelecimento.Endereco.Logradouro,
                    Numero = estabelecimento.Endereco.Numero,
                    Complemento = estabelecimento.Endereco.Complemento,
                    Bairro = estabelecimento.Endereco.Bairro,
                    Cidade = estabelecimento.Endereco.Cidade,
                    Estado = estabelecimento.Endereco.Estado
                }
            };

            return View(estabelecimentoViewModel);
        }

        // POST: Estabelecimento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EstabelecimentoViewModel estabelecimentoViewModel, EnderecoViewModel endereco)
        {
            if (ModelState.IsValid)
            {
                var estabelecimento = await _context.Estabelecimentos.FindAsync(id);
                if (estabelecimento == null) return NotFound();

                estabelecimento.Nome = estabelecimentoViewModel.Nome;
                estabelecimento.CNPJ = estabelecimentoViewModel.CNPJ;

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
                            _context.Enderecos.Update(enderecoExistente);
                        }
                    }
                    else
                    {
                        var novoEndereco = new Endereco
                        {
                            CEP = endereco.CEP,
                            Logradouro = endereco.Logradouro,
                            Numero = endereco.Numero,
                            Complemento = endereco.Complemento,
                            Bairro = endereco.Bairro,
                            Cidade = endereco.Cidade,
                            Estado = endereco.Estado
                        };
                        _context.Enderecos.Add(novoEndereco);
                        await _context.SaveChangesAsync();
                        estabelecimento.EnderecoId = novoEndereco.Id;
                    }
                }

                _context.Estabelecimentos.Update(estabelecimento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estabelecimentoViewModel);
        }

        // GET: Estabelecimento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var estabelecimento = await _context.Estabelecimentos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (estabelecimento == null) return NotFound();

            var estabelecimentoViewModel = new EstabelecimentoViewModel
            {
                Id = estabelecimento.Id,
                Nome = estabelecimento.Nome,
                CNPJ = estabelecimento.CNPJ,
                EnderecoId = estabelecimento.EnderecoId,
                Endereco = estabelecimento.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = estabelecimento.Endereco.Id,
                    CEP = estabelecimento.Endereco.CEP,
                    Logradouro = estabelecimento.Endereco.Logradouro,
                    Numero = estabelecimento.Endereco.Numero,
                    Complemento = estabelecimento.Endereco.Complemento,
                    Bairro = estabelecimento.Endereco.Bairro,
                    Cidade = estabelecimento.Endereco.Cidade,
                    Estado = estabelecimento.Endereco.Estado
                }
            };

            return View(estabelecimentoViewModel);
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