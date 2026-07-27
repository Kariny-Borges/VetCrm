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
    public class ProntuarioController : Controller
    {
        private readonly VetCrmContext _context;

        public ProntuarioController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Prontuario
        public async Task<IActionResult> Index()
        {
            var vetCrmContext = _context.Prontuarios.Include(p => p.Consulta).Include(p => p.Paciente);
            return View(await vetCrmContext.ToListAsync());
        }

        // GET: Prontuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prontuario = await _context.Prontuarios
                .Include(p => p.Consulta)
                    .ThenInclude(c => c.Paciente)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prontuario == null)
            {
                return NotFound();
            }

            return View(prontuario);
        }

        // GET: Prontuario/Atender/5   (o 5 é o Id da CONSULTA, não do prontuário)
        public async Task<IActionResult> Atender(int id)
        {
            // 1) Busca a consulta escolhida, trazendo o paciente e o prontuário (se já houver).
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Prontuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
            {
                return NotFound();
            }

            // 2) Se essa consulta ainda não tem prontuário, cria um em branco.
            //    (só na memória por enquanto — só salva no banco quando o vet clicar em Salvar)
            var prontuario = consulta.Prontuario ?? new Prontuario
            {
                ConsultaId = consulta.Id,
                PacienteId = consulta.PacienteId,
                DataRegistro = DateTime.Now
            };

            // 3) Histórico do paciente: consultas anteriores (menos a atual) e vacinas.
            var historicoConsultas = await _context.Consultas
                .Include(c => c.TipoConsulta)
                .Where(c => c.PacienteId == consulta.PacienteId && c.Id != consulta.Id)
                .OrderByDescending(c => c.DataConsulta)
                .ToListAsync();

            var historicoVacinas = await _context.PacienteVacinas
                .Include(pv => pv.Vacina)
                .Where(pv => pv.PacienteId == consulta.PacienteId)
                .OrderByDescending(pv => pv.DataAplicacao)
                .ToListAsync();

            var historicoTratamentos = await _context.Tratamentos
                .Where(t => t.PacienteId == consulta.PacienteId)
                .OrderByDescending(t => t.DataInicio)
                .ToListAsync();

            // Exames do paciente (traz o nome do exame do catálogo junto).
            var historicoExames = await _context.PacienteExames
                .Include(pe => pe.Exame)
                .Where(pe => pe.PacienteId == consulta.PacienteId)
                .OrderByDescending(pe => pe.DataSolicitacao)
                .ToListAsync();

            // Catálogo de exames pra montar o dropdown.
            var examesDisponiveis = await _context.Exames
                .OrderBy(e => e.Nome)
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nome })
                .ToListAsync();

            // Catálogo de vacinas pra montar o dropdown.
            var vacinasDisponiveis = await _context.Vacinas
                .OrderBy(v => v.Nome)
                .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.Nome })
                .ToListAsync();

            // 4) Retorna p/ tela
            var viewModel = new AtendimentoViewModel
            {
                Consulta = consulta,
                Prontuario = new AtenderProntuarioViewModel
                {
                    Id = prontuario.Id,
                    ConsultaId = consulta.Id,
                    Descricao = prontuario.Descricao,
                    Diagnostico = prontuario.Diagnostico
                },
                HistoricoConsultas = historicoConsultas,
                HistoricoVacinas = historicoVacinas,
                HistoricoTratamentos = historicoTratamentos,
                HistoricoExames = historicoExames,
                ExamesDisponiveis = examesDisponiveis,
                VacinasDisponiveis = vacinasDisponiveis,
                SomenteLeitura = consulta.Situacao == SituacaoConsulta.Realizada
            };

            return View(viewModel);
        }

        // POST: Prontuario/Atender
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atender(AtendimentoViewModel model)
        {
            var consultaId = model.Prontuario.ConsultaId;

            // Trava de segurança: se a consulta já foi Realizada, não deixa salvar
            // (mesmo que alguém tente mandar o form direto, sem passar pela tela).
            var consultaExistente = await _context.Consultas.FindAsync(consultaId);
            if (consultaExistente == null)
            {
                return NotFound();
            }
            if (consultaExistente.Situacao == SituacaoConsulta.Realizada)
            {
                return RedirectToAction(nameof(Atender), new { id = consultaId });
            }

            // Se o Id é 0, é um prontuário novo; senão, já existe e precisa atualizar.
            // PacienteId e DataRegistro saem da consulta/do banco, não da tela.
            Prontuario prontuario;
            if (model.Prontuario.Id == 0)
            {
                prontuario = new Prontuario
                {
                    ConsultaId = consultaId,
                    PacienteId = consultaExistente.PacienteId,
                    DataRegistro = DateTime.Now,
                    Descricao = model.Prontuario.Descricao,
                    Diagnostico = model.Prontuario.Diagnostico,
                    Tratamento = string.Empty
                };
                _context.Prontuarios.Add(prontuario);
            }
            else
            {
                var prontuarioExistente = await _context.Prontuarios.FindAsync(model.Prontuario.Id);
                if (prontuarioExistente == null)
                {
                    return NotFound();
                }

                prontuarioExistente.Descricao = model.Prontuario.Descricao;
                prontuarioExistente.Diagnostico = model.Prontuario.Diagnostico;
                prontuarioExistente.Tratamento ??= string.Empty;

                prontuario = prontuarioExistente;
            }

            // Se marcou "finalizar", muda a situação da consulta para Realizada.
            Consulta? consulta = null;
            if (model.FinalizarConsulta)
            {
                consulta = consultaExistente;
                consulta.Situacao = SituacaoConsulta.Realizada;
            }

            await _context.SaveChangesAsync();

            // Se o vet preencheu os campos do novo tratamento, adiciona na lista do paciente.
            if (!string.IsNullOrWhiteSpace(model.NovoTratamento?.Descricao))
            {
                var tratamento = new Tratamento
                {
                    Descricao = model.NovoTratamento.Descricao,
                    DataInicio = model.NovoTratamento.DataInicio == default ? DateTime.Now : model.NovoTratamento.DataInicio,
                    DataFim = model.NovoTratamento.DataFim,
                    PacienteId = prontuario.PacienteId,
                    ProntuarioId = prontuario.Id
                };
                _context.Tratamentos.Add(tratamento);
                await _context.SaveChangesAsync();
            }

            // Se o vet escolheu um exame no dropdown, adiciona na lista do paciente.
            if (model.NovoExame?.ExameId > 0)
            {
                var pacienteExame = new PacienteExame
                {
                    ExameId = model.NovoExame.ExameId,
                    DataSolicitacao = DateTime.Now,
                    PacienteId = prontuario.PacienteId,
                    ProntuarioId = prontuario.Id
                };
                _context.PacienteExames.Add(pacienteExame);
                await _context.SaveChangesAsync();
            }

            // Se o vet escolheu uma vacina no dropdown, aplica na lista do paciente.
            if (model.NovaVacina?.VacinaId > 0)
            {
                var pacienteVacina = new PacienteVacina
                {
                    VacinaId = model.NovaVacina.VacinaId,
                    DataAplicacao = model.NovaVacina.DataAplicacao == default ? DateTime.Now : model.NovaVacina.DataAplicacao,
                    DataProximaDose = model.NovaVacina.DataProximaDose,
                    PacienteId = prontuario.PacienteId,
                    ProntuarioId = prontuario.Id
                };
                _context.PacienteVacinas.Add(pacienteVacina);
                await _context.SaveChangesAsync();
            }

            // Se finalizou, volta pra Agenda (mesmo vet e dia). Senão, fica na tela pra seguir editando.
            if (model.FinalizarConsulta && consulta != null)
            {
                return RedirectToAction("Index", "Agenda", new
                {
                    veterinarioId = consulta.VeterinarioId,
                    data = consulta.DataConsulta.ToString("yyyy-MM-dd")
                });
            }

            return RedirectToAction(nameof(Atender), new { id = prontuario.ConsultaId });
        }

        // GET: Prontuario/EditarTratamento/5   (5 = Id do TRATAMENTO)
        // O consultaId vem junto só pra sabermos pra qual atendimento voltar depois.
        public async Task<IActionResult> EditarTratamento(int id, int consultaId)
        {
            var tratamento = await _context.Tratamentos.FindAsync(id);
            if (tratamento == null)
            {
                return NotFound();
            }

            var model = new EditarTratamentoViewModel
            {
                Id = tratamento.Id,
                Descricao = tratamento.Descricao,
                DataInicio = tratamento.DataInicio,
                DataFim = tratamento.DataFim
            };

            ViewBag.ConsultaId = consultaId;
            return View(model);
        }

        // POST: Prontuario/EditarTratamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarTratamento(int consultaId, EditarTratamentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tratamento = await _context.Tratamentos.FindAsync(model.Id);
                if (tratamento == null)
                {
                    return NotFound();
                }

                // Só o que o vet realmente edita. PacienteId e ProntuarioId
                // continuam com o valor que já está no banco.
                tratamento.Descricao = model.Descricao;
                tratamento.DataInicio = model.DataInicio;
                tratamento.DataFim = model.DataFim;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Atender), new { id = consultaId });
            }

            ViewBag.ConsultaId = consultaId;
            return View(model);
        }

        // GET: Prontuario/EditarExame/5   (5 = Id do PACIENTEEXAME)
        public async Task<IActionResult> EditarExame(int id, int consultaId)
        {
            var pacienteExame = await _context.PacienteExames.FindAsync(id);
            if (pacienteExame == null)
            {
                return NotFound();
            }

            var model = new PacienteExameViewModel
            {
                Id = pacienteExame.Id,
                ExameId = pacienteExame.ExameId,
                Resultado = pacienteExame.Resultado
            };

            ViewBag.ConsultaId = consultaId;
            ViewBag.ExamesDisponiveis = new SelectList(
                await _context.Exames.OrderBy(e => e.Nome).ToListAsync(),
                "Id", "Nome", pacienteExame.ExameId);
            return View(model);
        }

        // POST: Prontuario/EditarExame
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarExame(int consultaId, PacienteExameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pacienteExame = await _context.PacienteExames.FindAsync(model.Id);
                if (pacienteExame == null)
                {
                    return NotFound();
                }

                // Só o que o vet realmente edita. PacienteId, ProntuarioId e
                // DataSolicitacao continuam com o valor que já está no banco.
                pacienteExame.ExameId = model.ExameId;
                pacienteExame.Resultado = model.Resultado;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Atender), new { id = consultaId });
            }

            ViewBag.ConsultaId = consultaId;
            ViewBag.ExamesDisponiveis = new SelectList(
                await _context.Exames.OrderBy(e => e.Nome).ToListAsync(),
                "Id", "Nome", model.ExameId);
            return View(model);
        }

        // GET: Prontuario/EditarVacina/5   (5 = Id do PACIENTEVACINA)
        public async Task<IActionResult> EditarVacina(int id, int consultaId)
        {
            var pacienteVacina = await _context.PacienteVacinas.FindAsync(id);
            if (pacienteVacina == null)
            {
                return NotFound();
            }

            var model = new EditarVacinaViewModel
            {
                Id = pacienteVacina.Id,
                VacinaId = pacienteVacina.VacinaId,
                DataAplicacao = pacienteVacina.DataAplicacao,
                DataProximaDose = pacienteVacina.DataProximaDose
            };

            ViewBag.ConsultaId = consultaId;
            ViewBag.VacinasDisponiveis = new SelectList(
                await _context.Vacinas.OrderBy(v => v.Nome).ToListAsync(),
                "Id", "Nome", pacienteVacina.VacinaId);
            return View(model);
        }

        // POST: Prontuario/EditarVacina
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarVacina(int consultaId, EditarVacinaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pacienteVacina = await _context.PacienteVacinas.FindAsync(model.Id);
                if (pacienteVacina == null)
                {
                    return NotFound();
                }

                // Só o que o vet realmente edita. PacienteId e ProntuarioId
                // continuam com o valor que já está no banco.
                pacienteVacina.VacinaId = model.VacinaId;
                pacienteVacina.DataAplicacao = model.DataAplicacao;
                pacienteVacina.DataProximaDose = model.DataProximaDose;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Atender), new { id = consultaId });
            }

            ViewBag.ConsultaId = consultaId;
            ViewBag.VacinasDisponiveis = new SelectList(
                await _context.Vacinas.OrderBy(v => v.Nome).ToListAsync(),
                "Id", "Nome", model.VacinaId);
            return View(model);
        }

        // GET: Prontuario/Create
        public IActionResult Create()
        {
            ViewData["ConsultaId"] = new SelectList(_context.Consultas.Include(c => c.Paciente), "Id", "Resumo");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome");
            return View();
        }

        // POST: Prontuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProntuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var prontuario = new Prontuario
                {
                    Descricao = model.Descricao,
                    Diagnostico = model.Diagnostico,
                    Tratamento = model.Tratamento,
                    DataRegistro = model.DataRegistro,
                    PacienteId = model.PacienteId,
                    ConsultaId = model.ConsultaId
                };

                _context.Add(prontuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsultaId"] = new SelectList(_context.Consultas.Include(c => c.Paciente), "Id", "Resumo", model.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", model.PacienteId);
            return View(model);
        }

        // GET: Prontuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prontuario = await _context.Prontuarios.FindAsync(id);
            if (prontuario == null)
            {
                return NotFound();
            }
            var model = new ProntuarioViewModel
            {
                Id = prontuario.Id,
                Descricao = prontuario.Descricao,
                Diagnostico = prontuario.Diagnostico,
                Tratamento = prontuario.Tratamento,
                DataRegistro = prontuario.DataRegistro,
                PacienteId = prontuario.PacienteId,
                ConsultaId = prontuario.ConsultaId
            };

            ViewData["ConsultaId"] = new SelectList(_context.Consultas.Include(c => c.Paciente), "Id", "Resumo", model.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", model.PacienteId);
            return View(model);
        }

        // POST: Prontuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProntuarioViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var prontuario = await _context.Prontuarios.FindAsync(id);
                    if (prontuario == null)
                    {
                        return NotFound();
                    }

                    prontuario.Descricao = model.Descricao;
                    prontuario.Diagnostico = model.Diagnostico;
                    prontuario.Tratamento = model.Tratamento;
                    prontuario.DataRegistro = model.DataRegistro;
                    prontuario.PacienteId = model.PacienteId;
                    prontuario.ConsultaId = model.ConsultaId;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProntuarioExists(model.Id))
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
            ViewData["ConsultaId"] = new SelectList(_context.Consultas.Include(c => c.Paciente), "Id", "Resumo", model.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", model.PacienteId);
            return View(model);
        }

        // GET: Prontuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prontuario = await _context.Prontuarios
                .Include(p => p.Consulta)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prontuario == null)
            {
                return NotFound();
            }

            return View(prontuario);
        }

        // POST: Prontuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prontuario = await _context.Prontuarios.FindAsync(id);
            if (prontuario != null)
            {
                _context.Prontuarios.Remove(prontuario);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErroExclusao"] = "Não é possível excluir este prontuário porque ele possui registros vinculados.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProntuarioExists(int id)
        {
            return _context.Prontuarios.Any(e => e.Id == id);
        }
    }
}