using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZanganosSA.Data;
using ZanganosSA.Models;

namespace ZanganosSA.Controllers
{
    public class ColmenasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ColmenasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Colmenas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Colmenas.Include(c => c.Apiario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Colmenas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colmena = await _context.Colmenas
                .Include(c => c.Apiario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colmena == null)
            {
                return NotFound();
            }

            return View(colmena);
        }

        // GET: Colmenas/Create
        public IActionResult Create()
        {
            ViewData["ApiarioId"] = new SelectList(_context.Apiarios, "Id", "Nombre");
            return View();
        }

        // POST: Colmenas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Identificador,EstadoGeneral,EstadoReina,UltimaInspeccion,ApiarioId")] Colmena colmena)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colmena);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApiarioId"] = new SelectList(_context.Apiarios, "Id", "Nombre", colmena.ApiarioId);
            return View(colmena);
        }

        // GET: Colmenas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colmena = await _context.Colmenas.FindAsync(id);
            if (colmena == null)
            {
                return NotFound();
            }
            ViewData["ApiarioId"] = new SelectList(_context.Apiarios, "Id", "Nombre", colmena.ApiarioId);
            return View(colmena);
        }

        // POST: Colmenas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Identificador,EstadoGeneral,EstadoReina,UltimaInspeccion,ApiarioId")] Colmena colmena)
        {
            if (id != colmena.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colmena);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColmenaExists(colmena.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApiarioId"] = new SelectList(_context.Apiarios, "Id", "Nombre", colmena.ApiarioId);
            return View(colmena);
        }

        // GET: Colmenas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colmena = await _context.Colmenas
                .Include(c => c.Apiario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colmena == null)
            {
                return NotFound();
            }

            return View(colmena);
        }

        // POST: Colmenas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colmena = await _context.Colmenas.FindAsync(id);
            if (colmena != null)
            {
                _context.Colmenas.Remove(colmena);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColmenaExists(int id)
        {
            return _context.Colmenas.Any(e => e.Id == id);
        }
    }
}
