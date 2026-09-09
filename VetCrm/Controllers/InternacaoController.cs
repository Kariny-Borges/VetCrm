using Microsoft.AspNetCore.Mvc;
using VetCrm.Data;
using VetCrm.ViewModels;

namespace VetCrm.Controllers
{
    public class InternacaoController : Controller
    {
        private VetCrmContext _context;

        public InternacaoController(VetCrmContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var internacoes = _context.Internacoes.ToList();
            var internacoesViewModel = internacoes.Select(i => new InternacaoViewModel
            {
                DataEntrada = i.DataEntrada,
                DataSaida = i.DataSaida,
                Id = i.Id,
                Motivo = i.Motivo,
                PacienteId = i.PacienteId
            }).ToList();
            return View(internacoesViewModel);  
        }
    }
}
