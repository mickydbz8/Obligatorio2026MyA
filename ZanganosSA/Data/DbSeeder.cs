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
                // 1. Seed Apiarios (from SQL script Apiario inserts)
                var apiario1 = new Apiario 
                { 
                    Nombre = "Apiario Colonia", 
                    SeccionalPolicial = "5ta Seccional", 
                    UbicacionGps = "-34.46260000, -57.84000000" 
                };
                var apiario2 = new Apiario 
                { 
                    Nombre = "Apiario San José", 
                    SeccionalPolicial = "3ra Seccional", 
                    UbicacionGps = "-34.33672100, -56.71453200" 
                };
                var apiario3 = new Apiario 
                { 
                    Nombre = "Apiario Canelones", 
                    SeccionalPolicial = "1ra Seccional", 
                    UbicacionGps = "-34.52289100, -56.01874500" 
                };
                var apiario4 = new Apiario 
                { 
                    Nombre = "Apiario Flores", 
                    SeccionalPolicial = "2da Seccional", 
                    UbicacionGps = "-33.52801200, -56.87543200" 
                };

                context.Apiarios.AddRange(apiario1, apiario2, apiario3, apiario4);
                context.SaveChanges(); // Persist to get IDs

                // 2. Seed Colmenas (from SQL script Colmena inserts)
                var colmena101 = new Colmena { Identificador = "COL-101", EstadoGeneral = "Regular", EstadoReina = "Vieja y Débil", ApiarioId = apiario1.Id };
                var colmena102 = new Colmena { Identificador = "COL-102", EstadoGeneral = "Buena", EstadoReina = "Joven y Activa", ApiarioId = apiario1.Id };
                var colmena103 = new Colmena { Identificador = "COL-103", EstadoGeneral = "Regular", EstadoReina = "Joven y Activa", ApiarioId = apiario2.Id };
                var colmena104 = new Colmena { Identificador = "COL-104", EstadoGeneral = "Mala", EstadoReina = "Vieja y Débil", ApiarioId = apiario2.Id };
                var colmena105 = new Colmena { Identificador = "COL-105", EstadoGeneral = "Buena", EstadoReina = "Joven y Activa", ApiarioId = apiario3.Id };
                var colmena106 = new Colmena { Identificador = "COL-106", EstadoGeneral = "Mala", EstadoReina = "Ausente/Muerta", ApiarioId = apiario3.Id };
                var colmena107 = new Colmena { Identificador = "COL-107", EstadoGeneral = "Buena", EstadoReina = "Joven y Activa", ApiarioId = apiario4.Id };

                context.Colmenas.AddRange(colmena101, colmena102, colmena103, colmena104, colmena105, colmena106, colmena107);
                context.SaveChanges();

                // 3. Seed Tratamientos (from SQL script Tratam_sanitarios & Colmena_Tratamiento inserts)
                var trat1 = new Tratamiento { ColmenaId = colmena101.Id, Medicamento = "Timol", Dosis = "50ml", FechaInicio = new DateTime(2026, 05, 01), DuracionDias = 14, Estado = "Finalizado" };
                var trat2 = new Tratamiento { ColmenaId = colmena101.Id, Medicamento = "Apivar", Dosis = "2 tiras", FechaInicio = new DateTime(2026, 04, 01), DuracionDias = 56, Estado = "Finalizado" };
                var trat3 = new Tratamiento { ColmenaId = colmena102.Id, Medicamento = "Apivar", Dosis = "2 tiras", FechaInicio = new DateTime(2026, 04, 01), DuracionDias = 56, Estado = "Finalizado" };
                var trat4 = new Tratamiento { ColmenaId = colmena103.Id, Medicamento = "Oxabiol", Dosis = "5ml", FechaInicio = new DateTime(2026, 05, 15), DuracionDias = 3, Estado = "Finalizado" };
                var trat5 = new Tratamiento { ColmenaId = colmena104.Id, Medicamento = "Oxabiol", Dosis = "5ml", FechaInicio = new DateTime(2026, 05, 15), DuracionDias = 3, Estado = "Finalizado" };
                var trat6 = new Tratamiento { ColmenaId = colmena105.Id, Medicamento = "Api-Bioxal", Dosis = "3g", FechaInicio = new DateTime(2026, 05, 20), DuracionDias = 1, Estado = "Finalizado" };
                var trat7 = new Tratamiento { ColmenaId = colmena106.Id, Medicamento = "Api-Bioxal", Dosis = "3g", FechaInicio = new DateTime(2026, 05, 20), DuracionDias = 1, Estado = "Finalizado" };
                var trat8 = new Tratamiento { ColmenaId = colmena107.Id, Medicamento = "Timol", Dosis = "50ml", FechaInicio = new DateTime(2026, 05, 01), DuracionDias = 14, Estado = "Finalizado" };

                context.Tratamientos.AddRange(trat1, trat2, trat3, trat4, trat5, trat6, trat7, trat8);

                // 4. Seed Cosechas (from SQL script Cosecha inserts)
                var cosecha301 = new Cosecha { FechaCosecha = new DateTime(2026, 05, 20), PesoTotalEstimado = 250.50m, ApiarioId = apiario1.Id };
                var cosecha302 = new Cosecha { FechaCosecha = new DateTime(2025, 11, 15), PesoTotalEstimado = 380.00m, ApiarioId = apiario1.Id };
                var cosecha303 = new Cosecha { FechaCosecha = new DateTime(2026, 04, 10), PesoTotalEstimado = 420.50m, ApiarioId = apiario1.Id };
                var cosecha304 = new Cosecha { FechaCosecha = new DateTime(2026, 05, 25), PesoTotalEstimado = 195.00m, ApiarioId = apiario3.Id };

                context.Cosechas.AddRange(cosecha301, cosecha302, cosecha303, cosecha304);
                context.SaveChanges();

                // 5. Seed Barriles (from SQL script Barril_exportacion inserts)
                var barril401 = new Barril { CodigoLote = "LOTE-2026-A (Arg)", PesoNeto = 120.00m, FechaEnvasado = new DateTime(2026, 05, 20), CosechaId = cosecha301.Id };
                var barril402 = new Barril { CodigoLote = "LOTE-2026-A (Arg)", PesoNeto = 130.50m, FechaEnvasado = new DateTime(2026, 05, 20), CosechaId = cosecha301.Id };
                var barril403 = new Barril { CodigoLote = "LOTE-2025-B (Ger)", PesoNeto = 295.00m, FechaEnvasado = new DateTime(2025, 11, 15), CosechaId = cosecha302.Id };
                var barril404 = new Barril { CodigoLote = "LOTE-2025-B (Ger)", PesoNeto = 85.00m, FechaEnvasado = new DateTime(2025, 11, 15), CosechaId = cosecha302.Id };
                var barril405 = new Barril { CodigoLote = "LOTE-2026-B (USA)", PesoNeto = 300.00m, FechaEnvasado = new DateTime(2026, 04, 10), CosechaId = cosecha303.Id };
                var barril406 = new Barril { CodigoLote = "LOTE-2026-B (Jap)", PesoNeto = 120.50m, FechaEnvasado = new DateTime(2026, 04, 10), CosechaId = cosecha303.Id };

                context.Barriles.AddRange(barril401, barril402, barril403, barril404, barril405, barril406);

                // 6. Seed CosechaColmena relationships
                context.ColmenaCosechas.AddRange(
                    new ColmenaCosecha { CosechaId = cosecha301.Id, ColmenaId = colmena101.Id },
                    new ColmenaCosecha { CosechaId = cosecha302.Id, ColmenaId = colmena101.Id },
                    new ColmenaCosecha { CosechaId = cosecha302.Id, ColmenaId = colmena102.Id },
                    new ColmenaCosecha { CosechaId = cosecha302.Id, ColmenaId = colmena103.Id },
                    new ColmenaCosecha { CosechaId = cosecha303.Id, ColmenaId = colmena101.Id },
                    new ColmenaCosecha { CosechaId = cosecha303.Id, ColmenaId = colmena102.Id },
                    new ColmenaCosecha { CosechaId = cosecha303.Id, ColmenaId = colmena105.Id },
                    new ColmenaCosecha { CosechaId = cosecha304.Id, ColmenaId = colmena105.Id },
                    new ColmenaCosecha { CosechaId = cosecha304.Id, ColmenaId = colmena106.Id }
                );

                context.SaveChanges();
            }
        }
    }
}
