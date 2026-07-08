using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;
using VetCrm.Models.ViewModels;

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

            // 4) Retorna p/ tela
            var viewModel = new AtendimentoViewModel
            {
                Consulta = consulta,
                Prontuario = prontuario,
                HistoricoConsultas = historicoConsultas,
                HistoricoVacinas = historicoVacinas
            };

            return View(viewModel);
        }

        // POST: Prontuario/Atender
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atender(AtendimentoViewModel model)
        {
            var prontuario = model.Prontuario;

            // Se o Id é 0, é um prontuário novo; senão, já existe e precisa atualizar.
            if (prontuario.Id == 0)
            {
                _context.Prontuarios.Add(prontuario);
            }
            else
            {
                _context.Prontuarios.Update(prontuario);
            }

            // Se marcou "finalizar", muda a situação da consulta para Realizada.
            Consulta? consulta = null;
            if (model.FinalizarConsulta)
            {
                consulta = await _context.Consultas.FindAsync(prontuario.ConsultaId);
                if (consulta != null)
                {
                    consulta.Situacao = SituacaoConsulta.Realizada;
                }
            }

            await _context.SaveChangesAsync();

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
        public async Task<IActionResult> Create([Bind("Id,Descricao,Diagnostico,Tratamento,DataRegistro,PacienteId,ConsultaId")] Prontuario prontuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prontuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsultaId"] = new SelectList(_context.Consultas.Include(c => c.Paciente), "Id", "Resumo", prontuario.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", prontuario.PacienteId);
            return View(prontuario);
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
            ViewData["ConsultaId"] = new SelectList(_context.Consultas.Include(c => c.Paciente), "Id", "Resumo", prontuario.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", prontuario.PacienteId);
            return View(prontuario);
        }

        // POST: Prontuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Diagnostico,Tratamento,DataRegistro,PacienteId,ConsultaId")] Prontuario prontuario)
        {
            if (id != prontuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prontuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProntuarioExists(prontuario.Id))
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
            ViewData["ConsultaId"] = new SelectList(_context.Consultas.Include(c => c.Paciente), "Id", "Resumo", prontuario.ConsultaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", prontuario.PacienteId);
            return View(prontuario);
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
