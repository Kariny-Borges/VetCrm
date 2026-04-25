using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;

namespace VetCrm.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly VetCrmContext _context;

        public UsuarioController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Usuario - lista todos
        public async Task<IActionResult> Index()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Endereco);
            return View(await usuarios.ToListAsync());
        }

        // GET: Usuario/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Endereco)
                .Include(u => u.UsuarioEstabelecimentos)
                    .ThenInclude(ue => ue.Estabelecimento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null) return NotFound();

            ViewBag.Estabelecimentos = await _context.Estabelecimentos.ToListAsync();

            return View(usuario);
        }

        // GET: Usuario/Create - abre o formulario
        public async Task<IActionResult> Create()
        {
            ViewBag.Estabelecimentos = await _context.Estabelecimentos.ToListAsync();
            return View();
        }

        // POST: Usuario/Create - salva no banco
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Documento,Telefone,Email,Login,Senha,TipoPessoa,Perfil")] Usuario usuario, Endereco endereco, int? estabelecimentoId)
        {
            ModelState.Remove("Endereco");
            ModelState.Remove("UsuarioEstabelecimentos");
            ModelState.Remove("endereco.Id");

            if (ModelState.IsValid)
            {
                // Salva o endereco se pelo menos o logradouro foi preenchido
                if (!string.IsNullOrWhiteSpace(endereco.Logradouro))
                {
                    _context.Enderecos.Add(endereco);
                    await _context.SaveChangesAsync();
                    usuario.EnderecoId = endereco.Id;
                }

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Vincula estabelecimento se selecionado
                if (estabelecimentoId.HasValue && estabelecimentoId.Value > 0)
                {
                    _context.UsuarioEstabelecimentos.Add(new UsuarioEstabelecimento
                    {
                        UsuarioId = usuario.Id,
                        EstabelecimentoId = estabelecimentoId.Value,
                        DataVinculo = DateTime.Now
                    });
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Estabelecimentos = await _context.Estabelecimentos.ToListAsync();
            return View(usuario);
        }

        // GET: Usuario/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Endereco)
                .Include(u => u.UsuarioEstabelecimentos)
                    .ThenInclude(ue => ue.Estabelecimento)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return NotFound();

            ViewBag.Estabelecimentos = await _context.Estabelecimentos.ToListAsync();
            return View(usuario);
        }

        // POST: Usuario/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Documento,Telefone,Email,Login,Senha,TipoPessoa,Perfil,EnderecoId")] Usuario usuario, Endereco endereco)
        {
            if (id != usuario.Id) return NotFound();

            ModelState.Remove("Endereco");
            ModelState.Remove("UsuarioEstabelecimentos");
            ModelState.Remove("endereco.Id");

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualiza ou cria o endereco
                    if (!string.IsNullOrWhiteSpace(endereco.Logradouro))
                    {
                        if (usuario.EnderecoId.HasValue && usuario.EnderecoId.Value > 0)
                        {
                            // Atualiza endereco existente
                            var enderecoExistente = await _context.Enderecos.FindAsync(usuario.EnderecoId.Value);
                            if (enderecoExistente != null)
                            {
                                enderecoExistente.Logradouro = endereco.Logradouro;
                                enderecoExistente.Numero = endereco.Numero;
                                enderecoExistente.Bairro = endereco.Bairro;
                                enderecoExistente.Cidade = endereco.Cidade;
                                enderecoExistente.Estado = endereco.Estado;
                                enderecoExistente.CEP = endereco.CEP;
                            }
                        }
                        else
                        {
                            // Cria novo endereco
                            _context.Enderecos.Add(endereco);
                            await _context.SaveChangesAsync();
                            usuario.EnderecoId = endereco.Id;
                        }
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Estabelecimentos = await _context.Estabelecimentos.ToListAsync();
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // POST: Usuario/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null) _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Usuario/AdicionarEstabelecimento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarEstabelecimento(int usuarioId, int estabelecimentoId)
        {
            var jaExiste = _context.UsuarioEstabelecimentos
                .Any(ue => ue.UsuarioId == usuarioId && ue.EstabelecimentoId == estabelecimentoId);

            if (!jaExiste)
            {
                _context.UsuarioEstabelecimentos.Add(new UsuarioEstabelecimento
                {
                    UsuarioId = usuarioId,
                    EstabelecimentoId = estabelecimentoId,
                    DataVinculo = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = usuarioId });
        }

        // POST: Usuario/RemoverEstabelecimento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverEstabelecimento(int usuarioEstabelecimentoId, int usuarioId)
        {
            var vinculo = await _context.UsuarioEstabelecimentos.FindAsync(usuarioEstabelecimentoId);
            if (vinculo != null) _context.UsuarioEstabelecimentos.Remove(vinculo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = usuarioId });
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
