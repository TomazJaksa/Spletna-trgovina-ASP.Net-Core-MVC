using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voleska.Models;

namespace Voleska.Controllers
{
    public class OceneIzdelkovController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public OceneIzdelkovController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LajkaniBlogi
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.OceneIzdelkov.Include(l => l.Izdelek).Include(l => l.ApplicationUser);
            return View(await storeContext.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OddajOceno(byte selected_rating, string rate, int izdelekId, string imeTipaIzdelka)
        {
            if (selected_rating == 0)
            {
                TempData["TipIzdelka"] = imeTipaIzdelka;
                TempData["NiRatinga"] = "Izberite število zvezdic kot ocena zadovoljstva z izdelkom, kjer je 1 najmanj 5 pa največ.";
                TempData["ShranjenText"] = rate;
                return RedirectToAction("Details", "Katalog", new { id = izdelekId }, "preusmeri");

            }
            else {
                TempData["NiRatinga"] = "";

            }

            

            if (ModelState.IsValid)
            {
                OcenaIzdelka ocenaIzdelka = new OcenaIzdelka();
                ocenaIzdelka.Komentar = rate;
                ocenaIzdelka.Ocena = selected_rating;
                ocenaIzdelka.Dodan = DateTime.Now;
                ocenaIzdelka.Posodobljen = DateTime.Now;
                ocenaIzdelka.IzdelekID = izdelekId;
                ocenaIzdelka.ApplicationUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(ocenaIzdelka);
                await _context.SaveChangesAsync();

                var ustrezenIzdelek =await _context.Izdelki.FindAsync(izdelekId);

                var oceneIzdelkov = from m in _context.OceneIzdelkov
                               select m;

                var ustrezneOcene = oceneIzdelkov.Where(x => x.IzdelekID == izdelekId);

                List<OcenaIzdelka> seznamOcenIzdelkov = await ustrezneOcene.ToListAsync();

                int steviloOcen = seznamOcenIzdelkov.Count();
                int vsotaOcen = 0;

                foreach (var ocena in seznamOcenIzdelkov)
                {
                    vsotaOcen += ocena.Ocena;
                }

                int povprecje = vsotaOcen / steviloOcen;

                ustrezenIzdelek.PovprecnaOcena = povprecje;
                _context.Update(ustrezenIzdelek);

                await _context.SaveChangesAsync();


                TempData["TipIzdelka"] = imeTipaIzdelka;
                return RedirectToAction("Details", "Katalog", new { id = izdelekId }, "better-rating-form");
            }

            TempData["TipIzdelka"] = imeTipaIzdelka;
            return View("Izdelki");


        }

        // GET: OcenaIzdelka/Edit/5
        public async Task<IActionResult> Edit(int? id, string imeTipaIzdelka)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocenaIzdelka = await _context.OceneIzdelkov
                .Include(i => i.ApplicationUser)
                .Include(i => i.Izdelek)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (ocenaIzdelka == null)
            {
                return NotFound();
            }
            ViewData["TipIzdelka"] = imeTipaIzdelka;
            return View(ocenaIzdelka);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,string imeTipaIzdelka, byte selected_rating, string rate, int izdelekId)
        {
            var ocenaIzdelka = await _context.OceneIzdelkov.FindAsync(id);
            if (ocenaIzdelka == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                ocenaIzdelka.Komentar = rate;
                ocenaIzdelka.Ocena = selected_rating;
                ocenaIzdelka.Posodobljen = DateTime.Now;
                ocenaIzdelka.IzdelekID = izdelekId;
                ocenaIzdelka.ApplicationUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Update(ocenaIzdelka);
                await _context.SaveChangesAsync();

                var ustrezenIzdelek = await _context.Izdelki.FindAsync(izdelekId);

                var oceneIzdelkov = from m in _context.OceneIzdelkov
                                    select m;

                var ustrezneOcene = oceneIzdelkov.Where(x => x.IzdelekID == izdelekId);

                List<OcenaIzdelka> seznamOcenIzdelkov = await ustrezneOcene.ToListAsync();

                int steviloOcen = seznamOcenIzdelkov.Count();
                int vsotaOcen = 0;

                foreach (var ocena in seznamOcenIzdelkov)
                {
                    vsotaOcen += ocena.Ocena;
                }

                int povprecje = vsotaOcen / steviloOcen;

                ustrezenIzdelek.PovprecnaOcena = povprecje;
                _context.Update(ustrezenIzdelek);

                await _context.SaveChangesAsync();
                TempData["TipIzdelka"] = imeTipaIzdelka;
                return RedirectToAction("Details", "Katalog", new { id = izdelekId }, "ocene-in-komentarji");
            }

            return View("Izdelki");


        }


        // GET: OcenaIzdelka/Delete/5
        public async Task<IActionResult> Delete(int? id, string imeTipaIzdelka, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocenaIzdelka = await _context.OceneIzdelkov
                .Include(i => i.ApplicationUser)
                .Include(i => i.Izdelek)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ocenaIzdelka == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            ViewData["TipIzdelka"] = imeTipaIzdelka;
            return View(ocenaIzdelka);
        }

        // POST: Izdelki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int izdelekId, string imeTipaIzdelka)
        {
            var ocenaIzdelka = await _context.OceneIzdelkov.FindAsync(id);
            if (ocenaIzdelka == null)
            {
                TempData["TipIzdelka"] = imeTipaIzdelka;
                return RedirectToAction("Details", "Katalog", new { id = izdelekId }, "ocene-in-komentarji");
            }

            try
            {
                _context.OceneIzdelkov.Remove(ocenaIzdelka);
                await _context.SaveChangesAsync();

                var ustrezenIzdelek = await _context.Izdelki.FindAsync(izdelekId);

                var oceneIzdelkov = from m in _context.OceneIzdelkov
                                    select m;

                var ustrezneOcene = oceneIzdelkov.Where(x => x.IzdelekID == izdelekId);

                List<OcenaIzdelka> seznamOcenIzdelkov = await ustrezneOcene.ToListAsync();

                int steviloOcen = seznamOcenIzdelkov.Count();

                if (steviloOcen == 0) {
                    ustrezenIzdelek.PovprecnaOcena = 0;
                    _context.Update(ustrezenIzdelek);

                    await _context.SaveChangesAsync();

                    TempData["TipIzdelka"] = imeTipaIzdelka;
                    return RedirectToAction("Details", "Katalog", new { id = izdelekId }, "ocene-in-komentarji");

                }

                int vsotaOcen = 0;

                foreach (var ocena in seznamOcenIzdelkov)
                {
                    vsotaOcen += ocena.Ocena;
                }

                
                int povprecje = vsotaOcen / steviloOcen;

                ustrezenIzdelek.PovprecnaOcena = povprecje;
                _context.Update(ustrezenIzdelek);

                await _context.SaveChangesAsync();

                TempData["TipIzdelka"] = imeTipaIzdelka;
                return RedirectToAction("Details", "Katalog", new { id = izdelekId }, "ocene-in-komentarji");

            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }

}
