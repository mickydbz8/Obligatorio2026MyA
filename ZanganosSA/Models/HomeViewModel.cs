using ZanganosSA.Models;

namespace ZanganosSA.Models
{
    public class HomeViewModel
    {
        public List<Tratamiento> TratamientosActivos { get; set; } = new List<Tratamiento>();
        public int TotalColmenas { get; set; }
        public int TotalApiarios { get; set; }
        public List<Colmena> ColmenasPorAlimentar { get; set; } = new List<Colmena>();
        public decimal ProduccionEstimadaFutura { get; set; }
        public decimal CostoTotalEstimado { get; set; }
    }
}
