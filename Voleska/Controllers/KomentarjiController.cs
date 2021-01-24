using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using FsCheck.Experimental;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Voleska.Data;
using Voleska.Models;

namespace Voleska.Controllers
{
    public class KomentarjiController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public KomentarjiController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Komentarji
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.Komentarji.Include(k => k.Blog).Include(k => k.ApplicationUser);
            return View(await storeContext.ToListAsync());
        }

        // GET: Komentarji/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentarji
                .Include(k => k.Blog)
                .Include(k => k.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // GET: Komentarji/Create
        public IActionResult Create()
        {
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "Naslov");
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Komentarji/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public async Task<IActionResult> Create(string vsebina, int blogId)
        {
            Komentar komentar = new Komentar();
            if (ModelState.IsValid)
            {
                komentar.ApplicationUserID =  this.User.FindFirstValue(ClaimTypes.NameIdentifier); 
                komentar.BlogID = blogId;
                komentar.Vsebina = vsebina; // ne prenese mi vrednosti !!!
                komentar.SteviloDislike = 0;
                komentar.SteviloLike = 0;
                komentar.Dodan = DateTime.Now;
                komentar.Posodobljen = DateTime.Now;

                _context.Add(komentar);
                await _context.SaveChangesAsync();

                LajkanjeKomentarjev lajkanjeKomentarja = new LajkanjeKomentarjev();

                lajkanjeKomentarja.KomentarID = komentar.ID;
                lajkanjeKomentarja.Dodan = DateTime.Now;
                lajkanjeKomentarja.Posodobljen = DateTime.Now;
                lajkanjeKomentarja.Lajk = null;
                lajkanjeKomentarja.ApplicationUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(lajkanjeKomentarja);

                await _context.SaveChangesAsync();

                var komentarId = komentar.ID;

                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovKomentarVC), new { komentarId = komentarId });
                /*return RedirectToAction("Details", "Blogi", new { @id = blogId });*/

            }
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "ID", komentar.BlogID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "UserName");
            return RedirectToAction(nameof(Index));

        }

        public IActionResult SeznamReload(int komentarId, int blogId)
        {
            ViewData["UserID"] = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.SeznamKomentarjevVC), new {  komentarId = komentarId,  blogId=blogId });
        }

        public IActionResult VnosReload()
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.VnosKomentarjaVC));
        }

        // GET: Komentarji/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentarji.FindAsync(id);
            if (komentar == null)
            {
                return NotFound();
            }
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "ID", komentar.BlogID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", komentar.ApplicationUserID);
            return View(komentar);
        }

        // POST: Komentarji/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Vsebina,SteviloLike,SteviloDislike,Dodan,Posodobljen,UporabnikID,BlogID")] Komentar komentar)
        {
            if (id != komentar.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(komentar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KomentarExists(komentar.ID))
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
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "ID", komentar.BlogID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", komentar.ApplicationUserID);
            return View(komentar);
        }

        // GET: Komentarji/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentarji
                .Include(k => k.Blog)
                .Include(k => k.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // POST: Komentarji/Delete/5
        [HttpPost]
        
        public async Task<IActionResult> DeleteConfirmed(int id, int blogId)
        {
            var komentar = await _context.Komentarji.FindAsync(id);

            var lajkiOdKomentarja = from m in _context.LajkaniKomentarji
                              select m;

            lajkiOdKomentarja = lajkiOdKomentarja.Where(s => s.KomentarID == id );

            foreach (var lajk in lajkiOdKomentarja) {
                _context.LajkaniKomentarji.Remove(lajk);
            }
            await _context.SaveChangesAsync();

            _context.Komentarji.Remove(komentar);
            await _context.SaveChangesAsync();

            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.IzbrisiKomentarVC), new { komentarId = id });
            /*return RedirectToAction("Details", "Blogi", new { id = blogId }, "preusmeriNaSeznam");*/
        }

        private bool KomentarExists(int id)
        {
            return _context.Komentarji.Any(e => e.ID == id);
        }
    }
}
