using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace SafariDeDatos.Services
{
    public class SafariRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SafariRepository> _logger;

        public SafariRepository(IConfiguration configuration, ILogger<SafariRepository> logger)
        {
            _logger = logger;
            string? cs = configuration.GetConnectionString("Safari");
            if (cs == null)
            {
                _logger.LogError("Connection string 'Safari' not found in appsettings.json");
                throw new InvalidOperationException("Falta el connection string 'Safari' en appsettings.json");
            }
            _connectionString = cs;
            _logger.LogInformation("Connection string loaded successfully.");
        }

        public long ProbarConexion()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT 1", conn);
                object? result = cmd.ExecuteScalar();
                return Convert.ToInt64(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando SELECT 1");
                throw;
            }
        }

        public List<Dictionary<string, object>> Consultar(string sql)
        {
            var filas = new List<Dictionary<string, object>>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var fila = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columna = reader.GetName(i);
                    object valor = reader.GetValue(i);
                    fila[columna] = valor;
                }
                filas.Add(fila);
            }
            return filas;
        }
    }
}
