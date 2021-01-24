using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Voleska.Data;
using Voleska.Models;

namespace Voleska.Controllers
{
    public class TipiOpcijController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public TipiOpcijController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipiOpcij
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipiOpcij.ToListAsync());
        }

        // GET: TipiOpcij/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipOpcije = await _context.TipiOpcij
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipOpcije == null)
            {
                return NotFound();
            }

            return View(tipOpcije);
        }

        // GET: TipiOpcij/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipiOpcij/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ime")] TipOpcije tipOpcije)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipOpcije);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipOpcije);
        }

        // GET: TipiOpcij/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipOpcije = await _context.TipiOpcij.FindAsync(id);
            if (tipOpcije == null)
            {
                return NotFound();
            }
            return View(tipOpcije);
        }

        // POST: TipiOpcij/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ime")] TipOpcije tipOpcije)
        {
            if (id != tipOpcije.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipOpcije);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipOpcijeExists(tipOpcije.ID))
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
            return View(tipOpcije);
        }

        // GET: TipiOpcij/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipOpcije = await _context.TipiOpcij
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipOpcije == null)
            {
                return NotFound();
            }

            return View(tipOpcije);
        }

        // POST: TipiOpcij/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipOpcije = await _context.TipiOpcij.FindAsync(id);
            _context.TipiOpcij.Remove(tipOpcije);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipOpcijeExists(int id)
        {
            return _context.TipiOpcij.Any(e => e.ID == id);
        }
    }
}
