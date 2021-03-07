using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Voleska.Data;
using Voleska.Models;

namespace Voleska.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class TransakcijeController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public TransakcijeController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transakcije
        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, int? stevilkaStrani, int? kliknjenLinkStrani, string filtriranje, string izborTransakcij, string prejsnjaVrednostIzbora,  string izbira1, string izbira2, string izbira3)
        {
            var transakcije = from m in _context.Transakcije
                             select m;

            if (prejsnjaVrednostIzbora == null) {
                prejsnjaVrednostIzbora = izborTransakcij;
            }

            ViewData["VseTransakcije"] = "checked";
            ViewData["Prilagodi"] = "";

            if (searchString != null || searchString2 != null || searchString3 != null || filtriranje != null || izborTransakcij != prejsnjaVrednostIzbora || izbira1 != null || izbira2 != null || izbira3 != null)
            {
                stevilkaStrani = 1;

                if (kliknjenLinkStrani != null)
                {
                    stevilkaStrani = kliknjenLinkStrani;
                }

                

                switch (izborTransakcij)
                {
                    case "prilagodi":

                        ViewData["VseTransakcije"] = "";
                        ViewData["Prilagodi"] = "checked";

                        var zakljuceno = false;
                        var odposlano = false;
                        var vTeku = false;
                        var nicIzbrano = true;


                        if (izbira1 == "zakljuceno") {
                            zakljuceno = true;
                            ViewData["Zakljuceno"] = "zakljuceno";
                            nicIzbrano = false;
                        }

                        if (izbira2 == "odposlano") { 
                            odposlano = true;
                            ViewData["Odposlano"] = "odposlano";
                            nicIzbrano = false;
                        }

                        if (izbira3 == "vTeku")
                        {
                            nicIzbrano = false;
                            vTeku = true;
                            ViewData["VTeku"] = "vTeku";
                        }
                        
                        if (nicIzbrano==false) {
                            if (zakljuceno == true && odposlano == true && vTeku == true)
                            {

                                transakcije = transakcije.Where(s => s.Zakljuceno == true && s.Odposlano == true || (s.Zakljuceno == true && s.Odposlano == false) || (s.Zakljuceno == false && s.Odposlano == false));

                            }
                            else if (zakljuceno == true && odposlano == true && vTeku == false)
                            {

                                transakcije = transakcije.Where(s => s.Zakljuceno == true && s.Odposlano == true || (s.Zakljuceno==true && s.Odposlano==false));

                            }
                            else if (zakljuceno == true && vTeku == true && odposlano == false)
                            {

                                transakcije = transakcije.Where(s => s.Zakljuceno == true && s.Odposlano == false || (s.Zakljuceno == false && s.Odposlano == false));

                            }
                            else if (odposlano == true && vTeku == true && zakljuceno == false)
                            {

                                transakcije = transakcije.Where(s => s.Zakljuceno == true && s.Odposlano == true || (s.Zakljuceno == false && s.Odposlano == false));
                            }
                            else if (zakljuceno == true && odposlano == false && vTeku == false) {
                                transakcije = transakcije.Where(s => s.Zakljuceno == true && s.Odposlano == false);
                            } else if (odposlano == true && zakljuceno == false && vTeku == false) {
                                transakcije = transakcije.Where(s => s.Zakljuceno == true && s.Odposlano == true);
                            } else if (vTeku=true && odposlano==false && zakljuceno== false) {
                                transakcije = transakcije.Where(s => s.Zakljuceno == false && s.Odposlano == false);
                            }
                        }

                        break;
                }
                prejsnjaVrednostIzbora = izborTransakcij;

                if (!String.IsNullOrEmpty(searchString))
                {
                    transakcije = transakcije.Where(s => s.ApplicationUser.UserName.Contains(searchString));

                }
                if (!String.IsNullOrEmpty(searchString2))
                {
                    transakcije = transakcije.Where(s => s.ApplicationUser.Ime.Contains(searchString2));

                }
                if (!String.IsNullOrEmpty(searchString3))
                {
                    transakcije = transakcije.Where(s => s.ApplicationUser.Priimek.Contains(searchString3));

                }

                switch (filtriranje)
                {
                    case "datumPadajoce":
                        transakcije = transakcije.OrderByDescending(s => s.Dodan);
                        ViewData["IzbranFilter"] = "Najnovejše";
                        break;
                    case "datumNarascajoce":
                        transakcije = transakcije.OrderBy(s => s.Dodan);
                        ViewData["IzbranFilter"] = "Najstarejše";
                        break;
                }

                
            }
            

            List<Transakcija> ustrezneTransakcije = await transakcije.Include(t => t.ApplicationUser).ToListAsync();

            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["IzborTransakcij"] = izborTransakcij;
            ViewData["PrejsnjaVrednostIzbora"] = prejsnjaVrednostIzbora;
            ViewData["Filtriranje"] = filtriranje;
            if (stevilkaStrani == null)
            {
                ViewData["StevilkaStrani"] = 1;
            }
            else
            {
                ViewData["StevilkaStrani"] = stevilkaStrani;
            }

            int steviloPrikazanihTransakcij = 5;

            return View(PaginatedList<Transakcija>.CreateAsync(ustrezneTransakcije, stevilkaStrani ?? 1, steviloPrikazanihTransakcij));

        }

        // GET: Transakcije/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transakcija = await _context.Transakcije
                .Include(t => t.ApplicationUser)
                    .ThenInclude(n => n.Naslov)
                    .ThenInclude(p => p.Posta)
                .Include(n => n.Narocila)
                    .ThenInclude(i => i.Izdelek)
                    .ThenInclude(i => i.Material)
                .Include(n => n.Narocila)
                    .ThenInclude(i => i.Opcija)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transakcija == null)
            {
                return NotFound();
            }

            return View(transakcija);
        }

        public async Task<IActionResult> Odposlji(int? id, int? stevilkaStrani, string searchString, string searchString2, string searchString3, string filtriranje, string izborTransakcij, string prejsnjaVrednostIzbora, string izbira1, string izbira2, string izbira3)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transakcija = await _context.Transakcije.FindAsync(id);
            if (transakcija == null)
            {
                return NotFound();
            }

            try
            {
                transakcija.Odposlano = true;
                _context.Update(transakcija);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransakcijaExists(transakcija.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "Transakcije", new
            {
                stevilkaStrani = stevilkaStrani,
                searchString = searchString,
                searchString2 = searchString2,
                searchString3 = searchString3,
                filtriranje = filtriranje,
                izborTransakcij = izborTransakcij,
                prejsnjaVrednostIzbora = prejsnjaVrednostIzbora,
                izbira1 = izbira1,
                izbira2 = izbira2,
                izbira3 = izbira3

            }, $"transakcija-{id}");

        }

        public async Task<IActionResult> Razveljavi(int? id, int? stevilkaStrani, string searchString, string searchString2, string searchString3, string filtriranje, string izborTransakcij, string prejsnjaVrednostIzbora, string izbira1, string izbira2, string izbira3)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transakcija = await _context.Transakcije.FindAsync(id);
            if (transakcija == null)
            {
                return NotFound();
            }

            try
            {
                transakcija.Odposlano = false;
                _context.Update(transakcija);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransakcijaExists(transakcija.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "Transakcije", new
            {
                stevilkaStrani = stevilkaStrani,
                searchString = searchString,
                searchString2 = searchString2,
                searchString3 = searchString3,
                filtriranje = filtriranje,
                izborTransakcij= izborTransakcij,
                prejsnjaVrednostIzbora = prejsnjaVrednostIzbora,
                izbira1 = izbira1,
                izbira2 = izbira2,
                izbira3 = izbira3

            }, $"transakcija-{id}");

        }




        // GET: Transakcije/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Transakcije/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SkupniZnesek,Dodan,ApplicationUserID")] Transakcija transakcija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transakcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(transakcija);
        }

        // GET: Transakcije/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transakcija = await _context.Transakcije.FindAsync(id);
            if (transakcija == null)
            {
                return NotFound();
            }
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", transakcija.ApplicationUserID);
            return View(transakcija);
        }

        // POST: Transakcije/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SkupniZnesek,Dodan,ApplicationUserID")] Transakcija transakcija)
        {
            if (id != transakcija.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transakcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransakcijaExists(transakcija.ID))
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
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", transakcija.ApplicationUserID);
            return View(transakcija);
        }

        // GET: Transakcije/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transakcija = await _context.Transakcije
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transakcija == null)
            {
                return NotFound();
            }

            return View(transakcija);
        }

        // POST: Transakcije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transakcija = await _context.Transakcije.FindAsync(id);
            _context.Transakcije.Remove(transakcija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransakcijaExists(int id)
        {
            return _context.Transakcije.Any(e => e.ID == id);
        }
    }
}
