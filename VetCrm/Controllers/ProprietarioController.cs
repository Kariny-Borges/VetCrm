using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;


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
        public async Task<IActionResult> Create([Bind("Id,Nome,CPF,DataCadastro")] Proprietario proprietario, Endereco endereco)
        {
            ModelState.Remove("Endereco");
            ModelState.Remove("Contatos");
            ModelState.Remove("Pacientes");
            ModelState.Remove("endereco.Id");
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(endereco.Logradouro))
                {
                    _context.Enderecos.Add(endereco);
                    await _context.SaveChangesAsync();
                    proprietario.EnderecoId = endereco.Id;
                }

                _context.Add(proprietario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proprietario);
        }

        // GET: Proprietario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proprietario = await _context.Proprietarios.Include(p => p.Endereco).FirstOrDefaultAsync(p => p.Id == id);
            if (proprietario == null) return NotFound();

            return View(proprietario);
        }

        // POST: Proprietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CPF,DataCadastro,EnderecoId")] Proprietario proprietario, Endereco endereco)
        {
            if (id != proprietario.Id) return NotFound();

            ModelState.Remove("Endereco");
            ModelState.Remove("Contatos");
            ModelState.Remove("Pacientes");
            ModelState.Remove("endereco.Id");

            if (ModelState.IsValid)
            {
                try
                {
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
                            _context.Enderecos.Add(endereco);
                            await _context.SaveChangesAsync();
                            proprietario.EnderecoId = endereco.Id;
                        }
                    }

                    _context.Update(proprietario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProprietarioExists(proprietario.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proprietario);
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
