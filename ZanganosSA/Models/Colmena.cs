using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZanganosSA.Models
{
    public class Colmena
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Identificador Único")]
        public string Identificador { get; set; } = string.Empty;

        [Display(Name = "Estado General")]
        public string EstadoGeneral { get; set; } = "Buena";

        [Display(Name = "Estado de la Reina")]
        public string EstadoReina { get; set; } = "Joven y Activa";

        [Display(Name = "Fecha de Última Inspección")]
        public DateTime UltimaInspeccion { get; set; } = DateTime.Now;

        // Relación N a 1 con Apiario
        [Required]
        [ForeignKey("Apiario")]
        public int ApiarioId { get; set; }
        public Apiario? Apiario { get; set; }

        // Relación 1 a N con Tratamientos
        public ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();

        // Relaciones N a M manual con Cosechas
        public ICollection<ColmenaCosecha> ColmenaCosechas { get; set; } = new List<ColmenaCosecha>();
    }
}
