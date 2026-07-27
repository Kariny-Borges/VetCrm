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
    public class ProprietarioController : Controller
    {
        private readonly VetCrmContext _context;

        public ProprietarioController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Proprietario
        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Proprietarios.Include(p => p.Endereco).AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(p => p.Nome.Contains(busca) || p.CPF.Contains(busca));
            }

            ViewData["BuscaAtual"] = busca;
            return View(await query.ToListAsync());
        }

        // GET: Proprietario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietario = await _context.Proprietarios
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proprietario == null)
            {
                return NotFound();
            }

            return View(proprietario);
        }

        // GET: Proprietario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proprietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProprietarioViewModel model, EnderecoViewModel endereco)
        {
            if (ModelState.IsValid)
            {
                var proprietario = new Proprietario
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    DataCadastro = model.DataCadastro
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
                    proprietario.EnderecoId = novoEndereco.Id;
                }

                _context.Add(proprietario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Proprietario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proprietario = await _context.Proprietarios.Include(p => p.Endereco).FirstOrDefaultAsync(p => p.Id == id);
            if (proprietario == null) return NotFound();

            var model = new ProprietarioViewModel
            {
                Id = proprietario.Id,
                Nome = proprietario.Nome,
                CPF = proprietario.CPF,
                DataCadastro = proprietario.DataCadastro,
                EnderecoId = proprietario.EnderecoId,
                Endereco = proprietario.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = proprietario.Endereco.Id,
                    CEP = proprietario.Endereco.CEP,
                    Logradouro = proprietario.Endereco.Logradouro,
                    Numero = proprietario.Endereco.Numero,
                    Complemento = proprietario.Endereco.Complemento,
                    Bairro = proprietario.Endereco.Bairro,
                    Cidade = proprietario.Endereco.Cidade,
                    Estado = proprietario.Endereco.Estado
                }
            };

            return View(model);
        }

        // POST: Proprietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProprietarioViewModel model, EnderecoViewModel endereco)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var proprietario = await _context.Proprietarios.FindAsync(id);
                    if (proprietario == null) return NotFound();

                    proprietario.Nome = model.Nome;
                    proprietario.CPF = model.CPF;
                    proprietario.DataCadastro = model.DataCadastro;

                    if (!string.IsNullOrWhiteSpace(endereco.Logradouro))
                    {
                        if (proprietario.EnderecoId.HasValue && proprietario.EnderecoId.Value > 0)
                        {
                            var enderecoExistente = await _context.Enderecos.FindAsync(proprietario.EnderecoId.Value);
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
                            proprietario.EnderecoId = novoEndereco.Id;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProprietarioExists(model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Proprietario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietario = await _context.Proprietarios
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proprietario == null)
            {
                return NotFound();
            }

            return View(proprietario);
        }

        // POST: Proprietario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proprietario = await _context.Proprietarios.FindAsync(id);
            if (proprietario != null)
            {
                _context.Proprietarios.Remove(proprietario);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir este proprietário porque ele possui pacientes vinculados. Remova os pacientes primeiro.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProprietarioExists(int id)
        {
            return _context.Proprietarios.Any(e => e.Id == id);
        }
    }
}