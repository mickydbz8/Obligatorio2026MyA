using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZanganosSA.Models
{
    public class Tratamiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Medicamento")]
        public string Medicamento { get; set; } = string.Empty;

        [Required]
        public string Dosis { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Duración (Días)")]
        public int DuracionDias { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "En Curso"; // "En Curso" o "Finalizado"

        // Relación N a 1 con Colmena
        [Required]
        [ForeignKey("Colmena")]
        public int ColmenaId { get; set; }
        public Colmena? Colmena { get; set; }
    }
}
