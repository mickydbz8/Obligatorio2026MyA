using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZanganosSA.Data;
using ZanganosSA.Models;

namespace ZanganosSA.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var limiteAlerta = DateTime.Now.AddHours(48);
        var colmenasEnAlerta = await _context.Colmenas
            .Include(c => c.Apiario)
            .Where(c => c.FechaProximaAlimentacion.HasValue && c.FechaProximaAlimentacion.Value <= limiteAlerta)
            .OrderBy(c => c.FechaProximaAlimentacion)
            .ToListAsync();

        var colmenasActivas = await _context.Colmenas.CountAsync(c => c.EstadoGeneral == "Buena" || c.EstadoGeneral == "Excelente");
        // Estimación: 30kg por colmena activa
        var produccionEstimada = colmenasActivas * 30.0m;

        var costoTotal = await _context.Colmenas.SumAsync(c => c.CostoMantenimiento);

        var viewModel = new HomeViewModel
        {
            TotalApiarios = await _context.Apiarios.CountAsync(),
            TotalColmenas = await _context.Colmenas.CountAsync(),
            TratamientosActivos = await _context.Tratamientos
                .Include(t => t.Colmena)
                .Where(t => t.Estado == "En Curso")
                .OrderBy(t => t.FechaInicio)
                .ToListAsync(),
            ColmenasPorAlimentar = colmenasEnAlerta,
            ProduccionEstimadaFutura = produccionEstimada,
            CostoTotalEstimado = costoTotal
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
