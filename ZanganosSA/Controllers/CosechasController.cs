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
    public class CosechasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CosechasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cosechas
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 2;
            var query = _context.Cosechas.Include(c => c.Apiario);
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (page < 1) page = 1;
            if (totalPages > 0 && page > totalPages) page = totalPages;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return View(items);
        }

        // GET: Cosechas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cosecha = await _context.Cosechas
                .Include(c => c.Apiario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cosecha == null)
            {
                return NotFound();
            }

            return View(cosecha);
        }

        // GET: Cosechas/Create
        public IActionResult Create()
        {
            ViewData["ApiarioId"] = new SelectList(_context.Apiarios, "Id", "Nombre");
            return View();
        }

        // POST: Cosechas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaCosecha,PesoTotalEstimado,ApiarioId")] Cosecha cosecha)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cosecha);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApiarioId"] = new SelectList(_context.Apiarios, "Id", "Nombre", cosecha.ApiarioId);
            return View(cosecha);
        }

        // GET: Cosechas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cosecha = await _context.Cosechas.FindAsync(id);
            if (cosecha == null)
            {
                return NotFound();
            }
            ViewData["ApiarioId"] = new SelectList(_context.Apiarios, "Id", "Nombre", cosecha.ApiarioId);
            return View(cosecha);
        }

        // POST: Cosechas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaCosecha,PesoTotalEstimado,ApiarioId")] Cosecha cosecha)
        {
            if (id != cosecha.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cosecha);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CosechaExists(cosecha.Id))
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
            ViewData["ApiarioId"] = new SelectList(_context.Apiarios, "Id", "Nombre", cosecha.ApiarioId);
            return View(cosecha);
        }

        // GET: Cosechas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cosecha = await _context.Cosechas
                .Include(c => c.Apiario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cosecha == null)
            {
                return NotFound();
            }

            return View(cosecha);
        }

        // POST: Cosechas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cosecha = await _context.Cosechas.FindAsync(id);
            if (cosecha != null)
            {
                _context.Cosechas.Remove(cosecha);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CosechaExists(int id)
        {
            return _context.Cosechas.Any(e => e.Id == id);
        }
    }
}
