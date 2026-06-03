using Microsoft.AspNetCore.Mvc;
using SafariDeDatos.Services;
using System.Collections.Generic;
using System;

namespace SafariDeDatos.Controllers
{
    public class HomeController : Controller
    {
        private readonly SafariRepository _repo;

        public HomeController(SafariRepository repo)
        {
            _repo = repo;
        }

        // Redirect root to the connection test page
        public IActionResult Index()
        {
            return RedirectToAction("ProbarConexion");
        }

        // Action to test connection
        public IActionResult ProbarConexion()
        {
            try
            {
                long resultado = _repo.ProbarConexion();
                ViewBag.Resultado = resultado;
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al conectar: {ex.Message}";
            }
            return View();
        }

        // GET: shows empty SQL console
        [HttpGet]
        public IActionResult Consola()
        {
            return View();
        }

        // POST: executes SQL and shows results
        [HttpPost]
        public IActionResult Consola(string sql)
        {
            ViewBag.Sql = sql;
            List<Dictionary<string, object>> filas = _repo.Consultar(sql);
            ViewBag.Filas = filas;
            return View();
        }
    }
}
