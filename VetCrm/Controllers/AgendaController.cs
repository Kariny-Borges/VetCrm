using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;

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
            // Se nenhuma data foi escolhida, usa hoje.
            var dia = data ?? DateTime.Today;

            // Monta o dropdown de veterinarios, ja deixando marcado o que foi escolhido.
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Nome", veterinarioId);

            // Guarda a data e o vet escolhidos para a tela conseguir mostrar de volta.
            ViewData["Data"] = dia;
            ViewData["VeterinarioId_Selecionado"] = veterinarioId;

            // Busca as consultas do vet escolhido, naquele dia, ordenadas por horario.
            var consultas = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.TipoConsulta)
                .Where(c => c.VeterinarioId == veterinarioId && c.DataConsulta.Date == dia.Date)
                .OrderBy(c => c.DataConsulta)
                .ToListAsync();

            return View(consultas);
        }
    }
}
