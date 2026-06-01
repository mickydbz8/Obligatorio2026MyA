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
    public class ApiariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Apiarios
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 2;
            var query = _context.Apiarios;
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

        // GET: Apiarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiario = await _context.Apiarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiario == null)
            {
                return NotFound();
            }

            return View(apiario);
        }

        // GET: Apiarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apiarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,UbicacionGps,SeccionalPolicial")] Apiario apiario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apiario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apiario);
        }

        // GET: Apiarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiario = await _context.Apiarios.FindAsync(id);
            if (apiario == null)
            {
                return NotFound();
            }
            return View(apiario);
        }

        // POST: Apiarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,UbicacionGps,SeccionalPolicial")] Apiario apiario)
        {
            if (id != apiario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apiario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiarioExists(apiario.Id))
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
            return View(apiario);
        }

        // GET: Apiarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiario = await _context.Apiarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiario == null)
            {
                return NotFound();
            }

            return View(apiario);
        }

        // POST: Apiarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiario = await _context.Apiarios.FindAsync(id);
            if (apiario != null)
            {
                _context.Apiarios.Remove(apiario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApiarioExists(int id)
        {
            return _context.Apiarios.Any(e => e.Id == id);
        }
    }
}
