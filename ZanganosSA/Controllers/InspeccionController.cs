using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZanganosSA.Data;
using ZanganosSA.Models;
using ZanganosSA.Services;

namespace ZanganosSA.Controllers
{
    public class InspeccionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InspeccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SimularVoz()
        {
            var asistente = new IAAsistenteService();
            
            // 1. Simula que el usuario habla
            string dictado = asistente.SimularDictadoVoz("audio_123");
            
            // 2. Simula que la IA procesa el texto
            var nlpResult = asistente.SimularProcesamientoLenguajeNatural(dictado);

            // 3. Simular que guardamos en la BD (buscamos la colmena o la creamos)
            var colmena = await _context.Colmenas.FirstOrDefaultAsync(c => c.Id == nlpResult.ColmenaId);
            if (colmena != null)
            {
                colmena.EstadoGeneral = nlpResult.EstadoGeneral;
                colmena.EstadoReina = nlpResult.EstadoReina;
                colmena.UltimaInspeccion = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            TempData["MensajeVoz"] = $"¡Dictado procesado exitosamente! Texto interpretado: '{dictado}'. Se actualizó la Colmena #{nlpResult.ColmenaId}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult SimularCamara()
        {
            var vision = new IAVisionComputacionalService();

            // 1. Simula que la cámara toma una foto y se envía al modelo
            var visionResult = vision.AnalizarEstadoMaterial("foto_cajon.jpg");

            // 2. Aquí normalmente crearíamos una alerta de mantenimiento predictivo
            
            TempData["MensajeCamara"] = $"Análisis completado (Confianza: {visionResult.NivelConfianza * 100}%). Tipo: {visionResult.TipoMaterial}. Condición: {visionResult.Condicion}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
