using System.ComponentModel.DataAnnotations;

namespace ZanganosSA.Models
{
    public class Apiario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre del Apiario")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ubicación (Coordenadas GPS)")]
        public string UbicacionGps { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Seccional Policial")]
        public string SeccionalPolicial { get; set; } = string.Empty;

        // Relación 1 a N con Colmenas
        public ICollection<Colmena> Colmenas { get; set; } = new List<Colmena>();

        // Relación 1 a N con Cosechas
        public ICollection<Cosecha> Cosechas { get; set; } = new List<Cosecha>();
    }
}
