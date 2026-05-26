using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZanganosSA.Models
{
    public class Barril
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Código de Lote / Identificador")]
        public string CodigoLote { get; set; } = string.Empty;

        [Display(Name = "Peso Neto (kg)")]
        public decimal PesoNeto { get; set; } = 300.0m; // Estándar de 300kg

        [Display(Name = "Fecha de Envasado")]
        public DateTime FechaEnvasado { get; set; } = DateTime.Now;

        // Relación N a 1 con Cosecha
        [Required]
        [ForeignKey("Cosecha")]
        public int CosechaId { get; set; }
        public Cosecha? Cosecha { get; set; }
    }
}
