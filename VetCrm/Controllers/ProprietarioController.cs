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
        public async Task<IActionResult> Index()
        {
            var vetCrmContext = _context.Proprietarios.Include(p => p.Endereco);
            return View(await vetCrmContext.ToListAsync());
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
            ViewData["EnderecoId"] = new SelectList(_context.Set<Endereco>(), "Id", "Id");
            return View();
        }

        // POST: Proprietario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CPF,DataCadastro,EnderecoId")] Proprietario proprietario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proprietario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnderecoId"] = new SelectList(_context.Set<Endereco>(), "Id", "Id", proprietario.EnderecoId);
            return View(proprietario);
        }

        // GET: Proprietario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietario = await _context.Proprietarios.FindAsync(id);
            if (proprietario == null)
            {
                return NotFound();
            }
            ViewData["EnderecoId"] = new SelectList(_context.Set<Endereco>(), "Id", "Id", proprietario.EnderecoId);
            return View(proprietario);
        }

        // POST: Proprietario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CPF,DataCadastro,EnderecoId")] Proprietario proprietario)
        {
            if (id != proprietario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proprietario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProprietarioExists(proprietario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnderecoId"] = new SelectList(_context.Set<Endereco>(), "Id", "Id", proprietario.EnderecoId);
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

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProprietarioExists(int id)
        {
            return _context.Proprietarios.Any(e => e.Id == id);
        }
    }
}
