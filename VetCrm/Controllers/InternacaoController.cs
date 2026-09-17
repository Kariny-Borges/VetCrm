using Microsoft.AspNetCore.Mvc;
using VetCrm.Data;
using VetCrm.ViewModels;
using Microsoft.EntityFrameworkCore;
using VetCrm.Models;


namespace VetCrm.Controllers
{
    public class InternacaoController : Controller
    {
        private VetCrmContext _context;

        public InternacaoController(VetCrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var internacoes = await _context.Internacoes.Include(i => i.Paciente).ToListAsync();
            var internacoesViewModel = internacoes.Select(i => new InternacaoViewModel

            {
                Id = i.Id,
                Motivo = i.Motivo,
                DataEntrada = i.DataEntrada,
                DataSaida = i.DataSaida,
                PacienteId = i.PacienteId,
                Paciente = i.Paciente == null ? null : new PacienteViewModel
                {
                    Id = i.Paciente.Id,
                    Nome = i.Paciente.Nome
                }
            }).ToList();
            return View(internacoesViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internacao = await _context.Internacoes
                .Include(i => i.Paciente)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (internacao == null)
            {
                return NotFound();
            }

            var internacaoViewModel = new InternacaoViewModel
            {
                Id = internacao.Id,
                Motivo = internacao.Motivo,
                DataEntrada = internacao.DataEntrada,
                DataSaida = internacao.DataSaida,
                PacienteId = internacao.PacienteId,
                Paciente = internacao.Paciente == null ? null : new PacienteViewModel
                {
                    Id = internacao.Paciente.Id,
                    Nome = internacao.Paciente.Nome
                }
            };

            return View(internacaoViewModel);
        }
    }
}
