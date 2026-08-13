using System;
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
    public class AgendaController : Controller
    {
        private readonly VetCrmContext _context;

        public AgendaController(VetCrmContext context)
        {
            _context = context;
        }

        // GET: Agenda
        public async Task<IActionResult> Index(int? veterinarioId, DateTime? data)
        {
            var dia = data ?? DateTime.Today;

            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Nome", veterinarioId);
            ViewData["Data"] = dia;
            ViewData["VeterinarioId_Selecionado"] = veterinarioId;

            var consultas = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.TipoConsulta)
                .Where(c => c.VeterinarioId == veterinarioId && c.DataConsulta.Date == dia.Date)
                .OrderBy(c => c.DataConsulta)
                .ToListAsync();

            var consultasViewModel = consultas.Select(c => new ConsultaViewModel
            {
                Id = c.Id,
                DataConsulta = c.DataConsulta,
                Situacao = c.Situacao,
                Observacoes = c.Observacoes,
                TipoConsultaId = c.TipoConsultaId,
                PacienteId = c.PacienteId,
                VeterinarioId = c.VeterinarioId,
                TipoConsulta = c.TipoConsulta == null ? null : new TipoConsultaViewModel
                {
                    Id = c.TipoConsulta.Id,
                    Nome = c.TipoConsulta.Nome
                },
                Paciente = c.Paciente == null ? null : new PacienteViewModel
                {
                    Id = c.Paciente.Id,
                    Nome = c.Paciente.Nome
                }
            }).ToList();

            return View(consultasViewModel);
        }
    }
}
