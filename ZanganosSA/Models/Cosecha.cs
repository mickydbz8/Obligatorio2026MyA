using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZanganosSA.Models
{
    public class Cosecha
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha de Cosecha")]
        public DateTime FechaCosecha { get; set; } = DateTime.Now;

        [Display(Name = "Peso Total Estimado (kg)")]
        public decimal PesoTotalEstimado { get; set; }

        // Relación N a 1 con Apiario
        [Required]
        [ForeignKey("Apiario")]
        public int ApiarioId { get; set; }
        public Apiario? Apiario { get; set; }

        public ICollection<ColmenaCosecha> ColmenaCosechas { get; set; } = new List<ColmenaCosecha>();

        public ICollection<Barril> Barriles { get; set; } = new List<Barril>();
    }

    // Tabla intermedia para relación N a M entre Colmena y Cosecha
    public class ColmenaCosecha
    {
        public int ColmenaId { get; set; }
        public Colmena? Colmena { get; set; }

        public int CosechaId { get; set; }
        public Cosecha? Cosecha { get; set; }
    }
}
