using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.ViewModels;

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
        public async Task<IActionResult> Index(string busca)
        {
            var query = _context.Usuarios
                .Include(u => u.Endereco)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                query = query.Where(u => u.Nome.Contains(busca) || u.Email.Contains(busca));
            }

            var usuarios = await query.ToListAsync();
            var usuariosViewModel = usuarios.Select(u => new UsuarioViewModel
            {
                Id = u.Id,
                Nome = u.Nome,
                CPF = u.CPF,
                Telefone = u.Telefone,
                Email = u.Email,
                Login = u.Login,
                Senha = u.Senha,
                Perfil = u.Perfil,
                EnderecoId = u.EnderecoId,
                Endereco = u.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = u.Endereco.Id,
                    CEP = u.Endereco.CEP,
                    Logradouro = u.Endereco.Logradouro,
                    Numero = u.Endereco.Numero,
                    Complemento = u.Endereco.Complemento,
                    Bairro = u.Endereco.Bairro,
                    Cidade = u.Endereco.Cidade,
                    Estado = u.Endereco.Estado
                }
            }).ToList();

            ViewData["BuscaAtual"] = busca;
            return View(usuariosViewModel);
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

            var usuarioViewModel = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                CPF = usuario.CPF,
                Telefone = usuario.Telefone,
                Email = usuario.Email,
                Login = usuario.Login,
                Senha = usuario.Senha,
                Perfil = usuario.Perfil,
                EnderecoId = usuario.EnderecoId,
                Endereco = usuario.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = usuario.Endereco.Id,
                    CEP = usuario.Endereco.CEP,
                    Logradouro = usuario.Endereco.Logradouro,
                    Numero = usuario.Endereco.Numero,
                    Complemento = usuario.Endereco.Complemento,
                    Bairro = usuario.Endereco.Bairro,
                    Cidade = usuario.Endereco.Cidade,
                    Estado = usuario.Endereco.Estado
                },
                UsuarioEstabelecimentos = usuario.UsuarioEstabelecimentos.Select(ue => new UsuarioEstabelecimentoViewModel
                {
                    Id = ue.Id,
                    UsuarioId = ue.UsuarioId,
                    EstabelecimentoId = ue.EstabelecimentoId,
                    Estabelecimento = ue.Estabelecimento == null ? null : new EstabelecimentoViewModel
                    {
                        Id = ue.Estabelecimento.Id,
                        Nome = ue.Estabelecimento.Nome,
                        CNPJ = ue.Estabelecimento.CNPJ
                    }
                }).ToList()
            };

            var estabelecimentos = await _context.Estabelecimentos.ToListAsync();
            ViewBag.Estabelecimentos = estabelecimentos.Select(e => new EstabelecimentoViewModel
            {
                Id = e.Id,
                Nome = e.Nome,
                CNPJ = e.CNPJ
            }).ToList();

            return View(usuarioViewModel);
        }

        // GET: Usuario/Create - abre o formulario
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create - salva no banco
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel model, EnderecoViewModel endereco)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = new Usuario
                    {
                        Nome = model.Nome,
                        CPF = model.CPF,
                        Telefone = model.Telefone,
                        Email = model.Email,
                        Login = model.Login,
                        Senha = model.Senha,
                        Perfil = model.Perfil
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
                        usuario.EnderecoId = novoEndereco.Id;
                    }

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Mostra na tela qualquer exceção que aconteça ao salvar (chave duplicada, etc.)
                    ModelState.AddModelError(string.Empty, "Erro ao salvar: " + (ex.InnerException?.Message ?? ex.Message));
                }
            }

            // Diagnóstico: junta todos os erros do ModelState pra exibir na view
            ViewBag.ErrosDebug = string.Join(" | ", ModelState
                .Where(x => x.Value!.Errors.Count > 0)
                .Select(x => $"{x.Key}: {x.Value!.Errors[0].ErrorMessage}"));

            return View(model);
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

            var model = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                CPF = usuario.CPF,
                Telefone = usuario.Telefone,
                Email = usuario.Email,
                Login = usuario.Login,
                Senha = usuario.Senha,
                Perfil = usuario.Perfil,
                EnderecoId = usuario.EnderecoId,
                Endereco = usuario.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = usuario.Endereco.Id,
                    CEP = usuario.Endereco.CEP,
                    Logradouro = usuario.Endereco.Logradouro,
                    Numero = usuario.Endereco.Numero,
                    Complemento = usuario.Endereco.Complemento,
                    Bairro = usuario.Endereco.Bairro,
                    Cidade = usuario.Endereco.Cidade,
                    Estado = usuario.Endereco.Estado
                }
            };

            ViewBag.Estabelecimentos = await _context.Estabelecimentos.ToListAsync();
            return View(model);
        }

        // POST: Usuario/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioViewModel model, EnderecoViewModel endereco)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = await _context.Usuarios.FindAsync(id);
                    if (usuario == null) return NotFound();

                    usuario.Nome = model.Nome;
                    usuario.CPF = model.CPF;
                    usuario.Telefone = model.Telefone;
                    usuario.Email = model.Email;
                    usuario.Login = model.Login;
                    usuario.Senha = model.Senha;
                    usuario.Perfil = model.Perfil;

                    if (!string.IsNullOrWhiteSpace(endereco.Logradouro))
                    {
                        if (usuario.EnderecoId.HasValue && usuario.EnderecoId.Value > 0)
                        {
                            var enderecoExistente = await _context.Enderecos.FindAsync(usuario.EnderecoId.Value);
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
                            usuario.EnderecoId = novoEndereco.Id;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Estabelecimentos = await _context.Estabelecimentos.ToListAsync();
            return View(model);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null) return NotFound();

            var usuarioViewModel = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                CPF = usuario.CPF,
                Telefone = usuario.Telefone,
                Email = usuario.Email,
                Login = usuario.Login,
                Senha = usuario.Senha,
                Perfil = usuario.Perfil,
                EnderecoId = usuario.EnderecoId,
                Endereco = usuario.Endereco == null ? null : new EnderecoViewModel
                {
                    Id = usuario.Endereco.Id,
                    CEP = usuario.Endereco.CEP,
                    Logradouro = usuario.Endereco.Logradouro,
                    Numero = usuario.Endereco.Numero,
                    Complemento = usuario.Endereco.Complemento,
                    Bairro = usuario.Endereco.Bairro,
                    Cidade = usuario.Endereco.Cidade,
                    Estado = usuario.Endereco.Estado
                }
            };

            return View(usuarioViewModel);
        }

        // POST: Usuario/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null) _context.Usuarios.Remove(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir este usuário porque ele possui estabelecimentos vinculados.";
                return RedirectToAction(nameof(Delete), new { id });
            }

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
                    EstabelecimentoId = estabelecimentoId
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