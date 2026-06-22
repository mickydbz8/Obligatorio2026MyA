using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZanganosSA.Data;
using ZanganosSA.Models;
using System.Text.RegularExpressions;

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

        [HttpGet]
        public async Task<IActionResult> GetColmenas()
        {
            var colmenas = await _context.Colmenas
                .Select(c => new { c.Id, c.Identificador, c.EstadoGeneral, c.EstadoReina })
                .ToListAsync();
            return Json(colmenas);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarVoz([FromBody] VoiceInput model)
        {
            if (string.IsNullOrEmpty(model.Texto))
            {
                return Json(new { success = false, message = "No se recibió ningún texto." });
            }

            // Parser de procesamiento de lenguaje natural (NLP) simple
            int colmenaId = model.ColmenaId;
            
            // Si el texto contiene un número, intentamos extraer el ID de la colmena
            var match = Regex.Match(model.Texto, @"colmena\s*(\d+)", RegexOptions.IgnoreCase);
            if (match.Success && int.TryParse(match.Groups[1].Value, out int parsedId))
            {
                // Verificar si existe la colmena con este ID en la BD
                var exists = await _context.Colmenas.AnyAsync(c => c.Id == parsedId);
                if (exists)
                {
                    colmenaId = parsedId;
                }
            }

            var colmena = await _context.Colmenas.FirstOrDefaultAsync(c => c.Id == colmenaId);
            if (colmena == null)
            {
                return Json(new { success = false, message = $"No se encontró la colmena seleccionada o mencionada." });
            }

            // Análisis de palabras clave para Estado General
            string estadoGeneral = colmena.EstadoGeneral;
            if (Regex.IsMatch(model.Texto, @"(crítico|critico|muy malo|pésimo|pesimo)", RegexOptions.IgnoreCase))
                estadoGeneral = "Mala";
            else if (Regex.IsMatch(model.Texto, @"(regular|regular|medio|así así)", RegexOptions.IgnoreCase))
                estadoGeneral = "Regular";
            else if (Regex.IsMatch(model.Texto, @"(bueno|buena|sano|saludable|excelente|perfecto)", RegexOptions.IgnoreCase))
                estadoGeneral = "Buena";

            // Análisis de palabras clave para Estado de la Reina
            string estadoReina = colmena.EstadoReina;
            if (Regex.IsMatch(model.Texto, @"(muerta|ausente|no está|no esta|sin reina)", RegexOptions.IgnoreCase))
                estadoReina = "Ausente/Muerta";
            else if (Regex.IsMatch(model.Texto, @"(vieja|débil|debil|lenta)", RegexOptions.IgnoreCase))
                estadoReina = "Vieja y Débil";
            else if (Regex.IsMatch(model.Texto, @"(joven|activa|buena reina|sana)", RegexOptions.IgnoreCase))
                estadoReina = "Joven y Activa";

            // Actualizar colmena
            colmena.EstadoGeneral = estadoGeneral;
            colmena.EstadoReina = estadoReina;
            colmena.UltimaInspeccion = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                colmenaId = colmena.Id,
                identificador = colmena.Identificador,
                estadoGeneral = colmena.EstadoGeneral,
                estadoReina = colmena.EstadoReina,
                mensaje = $"Colmena #{colmena.Id} ({colmena.Identificador}) actualizada: Estado General '{estadoGeneral}', Reina '{estadoReina}'."
            });
        }

        [HttpPost]
        public IActionResult ProcesarVision([FromBody] VisionInput model)
        {
            // Simular análisis visual inteligente dependiendo de la imagen cargada/seleccionada
            string tipoMaterial = "Cajón de Colmena";
            string condicion = "Buen Estado";
            double nivelConfianza = 0.94;
            var boundingBoxes = new List<object>();

            if (model.Preset == "beehive_inspection")
            {
                tipoMaterial = "Cajón de Colmena (Madera)";
                condicion = "Desgaste moderado y roturas menores detectadas. Se aconseja mantenimiento preventivo.";
                nivelConfianza = 0.89;
                boundingBoxes.Add(new { label = "Madera dañada", x = 120, y = 180, w = 220, h = 150, confidence = 0.89 });
                boundingBoxes.Add(new { label = "Grieta en base", x = 320, y = 300, w = 180, h = 80, confidence = 0.82 });
            }
            else if (model.Preset == "healthy_honeycomb")
            {
                tipoMaterial = "Panal / Cuadro de Cera";
                condicion = "Excelente densidad de población. Panal 100% saludable sin signos de varroosis.";
                nivelConfianza = 0.97;
                boundingBoxes.Add(new { label = "Abejas Saludables", x = 80, y = 50, w = 400, h = 320, confidence = 0.97 });
                boundingBoxes.Add(new { label = "Celdas de Miel Llenas", x = 150, y = 100, w = 180, h = 150, confidence = 0.92 });
            }
            else
            {
                // Caso genérico o capturado por cámara
                tipoMaterial = "Cajón e Instrumentos";
                condicion = "Análisis completado: Estructura estable. No se detectan anomalías críticas.";
                nivelConfianza = 0.85;
                boundingBoxes.Add(new { label = "Objeto Detectado", x = 100, y = 100, w = 300, h = 250, confidence = 0.85 });
            }

            return Json(new
            {
                success = true,
                tipoMaterial = tipoMaterial,
                condicion = condicion,
                nivelConfianza = nivelConfianza,
                boundingBoxes = boundingBoxes
            });
        }
    }

    public class VoiceInput
    {
        public int ColmenaId { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    public class VisionInput
    {
        public string Preset { get; set; } = string.Empty;
        public string Base64Image { get; set; } = string.Empty;
    }
}
