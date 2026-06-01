using Microsoft.EntityFrameworkCore;
using ZanganosSA.Models;

namespace ZanganosSA.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Apiarios.Any())
            {
                var apiario1 = new Apiario { Nombre = "Apiario Norte", UbicacionGps = "-34.9011, -56.1645", SeccionalPolicial = "Seccional 1" };
                var apiario2 = new Apiario { Nombre = "Apiario Sur", UbicacionGps = "-34.9122, -56.1755", SeccionalPolicial = "Seccional 2" };
                context.Apiarios.AddRange(apiario1, apiario2);
                context.SaveChanges();

                var colmena1 = new Colmena { Identificador = "COL-001", EstadoGeneral = "Fuerte", EstadoReina = "Joven", ApiarioId = apiario1.Id, UltimaInspeccion = DateTime.Now.AddDays(-10) };
                var colmena2 = new Colmena { Identificador = "COL-002", EstadoGeneral = "Débil", EstadoReina = "Vieja", ApiarioId = apiario1.Id, UltimaInspeccion = DateTime.Now.AddDays(-5) };
                var colmena3 = new Colmena { Identificador = "COL-003", EstadoGeneral = "Normal", EstadoReina = "Joven", ApiarioId = apiario2.Id, UltimaInspeccion = DateTime.Now.AddDays(-2) };
                context.Colmenas.AddRange(colmena1, colmena2, colmena3);
                context.SaveChanges();

                var trat1 = new Tratamiento { ColmenaId = colmena2.Id, Medicamento = "Oxálico", Dosis = "50g", FechaInicio = DateTime.Now.AddDays(-40), DuracionDias = 42, Estado = "En Curso" }; // A punto de vencer (faltan 2 días)
                var trat2 = new Tratamiento { ColmenaId = colmena3.Id, Medicamento = "Amitraz", Dosis = "2 Tiras", FechaInicio = DateTime.Now.AddDays(-60), DuracionDias = 45, Estado = "En Curso" }; // Vencido (hace 15 días)
                var trat3 = new Tratamiento { ColmenaId = colmena1.Id, Medicamento = "Flumetrina", Dosis = "4 Tiras", FechaInicio = DateTime.Now.AddDays(-10), DuracionDias = 30, Estado = "En Curso" };
                context.Tratamientos.AddRange(trat1, trat2, trat3);
                context.SaveChanges();
            }
        }
    }
}
