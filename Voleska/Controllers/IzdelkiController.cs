using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Voleska.Data;
using Voleska.Models;
using Voleska.Services;
using Voleska.ViewModel;

namespace Voleska.Controllers
{
    [Authorize(Roles ="ADMINISTRATOR")]
    public class IzdelkiController : Controller
    {
        
        private readonly Data.ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public IzdelkiController(Data.ApplicationDbContext context, IMailService mailService)
        {   
            _context = context;
            _mailService = mailService;

        }

        // GET: Izdelki
        public async Task<IActionResult> Index(string izbranTipIzdelka, string SearchString, string filtriranje, int? stevilkaStrani, int? kliknjenLinkStrani)
        {
            IQueryable<string> tipiIzdelka = (IQueryable<string>)(from m in _context.TipiIzdelkov
                                             orderby m.ID
                                             select m.Ime);

            var izdelki = from m in _context.Izdelki
                              select m;

            if (izbranTipIzdelka != null || SearchString != null || filtriranje != null) {

            
                stevilkaStrani = 1;

                if (kliknjenLinkStrani!=null) {
                    stevilkaStrani = kliknjenLinkStrani;
                }

                if (!String.IsNullOrEmpty(SearchString))
                {
                    izdelki = izdelki.Where(s => s.Ime.Contains(SearchString));

                }

                if (!string.IsNullOrEmpty(izbranTipIzdelka))
                {
                    izdelki = izdelki.Where(x => x.TipIzdelka.Ime == izbranTipIzdelka);
                }

                switch (filtriranje)
                {
                    case "cenaNarascajoce":
                        izdelki = izdelki.OrderBy(s => s.Cena);
                        ViewData["IzbranFilter"] = "Cena (Naraščajoče)";
                        break;
                    case "cenaPadajoce":
                        izdelki = izdelki.OrderByDescending(s => s.Cena);
                        ViewData["IzbranFilter"] = "Cena (Padajoče)";
                        break;
                    case "datumPadajoce":
                        izdelki = izdelki.OrderByDescending(s => s.Posodobljen);
                        ViewData["IzbranFilter"] = "Najnovejši";
                        break;
                    case "ocena":
                        izdelki = izdelki.OrderByDescending(s => s.PovprecnaOcena);
                        ViewData["IzbranFilter"] = "Ocena";
                        break;
                    case "priljubljenost":
                        izdelki = izdelki.OrderByDescending(s => s.SteviloProdanih);
                        ViewData["IzbranFilter"] = "Priljubljenost";
                        break;
                }
            }
            


            ViewData["TipIzdelka"] = izbranTipIzdelka;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["SearchString"] = SearchString;
            if (stevilkaStrani == null)
            {
                ViewData["StevilkaStrani"] = 1;
            }
            else {
                ViewData["StevilkaStrani"] = stevilkaStrani;
            }
            
        

            IzdelkiViewModel izdelkiTipiVM = new IzdelkiViewModel();
            int steviloPrikazanihIzdelkov = 5;

            izdelkiTipiVM.TipiIzdelka = new SelectList(await tipiIzdelka.Distinct().ToListAsync());
            var izdelki2 = await izdelki.Include(i => i.Material).Include(i => i.TipIzdelka).ToListAsync();
            izdelkiTipiVM.Izdelki = PaginatedList<Izdelek>.CreateAsync(izdelki2, stevilkaStrani ?? 1, steviloPrikazanihIzdelkov);


            return View(izdelkiTipiVM);
        }

        // GET: Izdelki/Details/5
        public async Task<IActionResult> Details(int? id, string izbranTipIzdelka, string SearchString, string filtriranje, int? stevilkaStrani)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izdelek = await _context.Izdelki
                .Include(i => i.Material)
                .Include(i => i.TipIzdelka)
                .Include(i => i.Opcije)
                .ThenInclude(i => i.TipOpcije)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (izdelek == null)
            {
                return NotFound();
            }

            ViewData["TipIzdelka"] = izbranTipIzdelka;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["SearchString"] = SearchString;
            ViewData["StevilkaStrani"] = stevilkaStrani;
            return View(izdelek);
        }

        // GET: Izdelki/Create
        public IActionResult Create(string izbranTipIzdelka, string SearchString, string filtriranje, int? stevilkaStrani)
        {
            ViewData["TipIzdelka"] = izbranTipIzdelka;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["SearchString"] = SearchString;
            ViewData["StevilkaStrani"] = stevilkaStrani;

            ViewData["MaterialID"] = new SelectList(_context.Materiali, "ID", "Ime");
            ViewData["TipIzdelkaID"] = new SelectList(_context.TipiIzdelkov, "ID", "Ime");
            return View();
        }

        // POST: Izdelki/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Opis,Podrobnosti,Cena,TipIzdelkaID")] Izdelek izdelek)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    izdelek.Izbrisan = false;
                    izdelek.MaterialID = 1;
                    izdelek.Dodan = DateTime.Now;
                    izdelek.Posodobljen = DateTime.Now;
                    izdelek.SteviloProdanih = 0;
                    izdelek.PovprecnaOcena = 0;
                    _context.Add(izdelek);
                    await _context.SaveChangesAsync();

                  
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            ViewData["MaterialID"] = new SelectList(_context.Materiali, "ID", "Ime");
            ViewData["TipIzdelkaID"] = new SelectList(_context.TipiIzdelkov, "ID", "Ime");
            return View(izdelek);
        }

        // GET: Izdelki/Edit/5
        public async Task<IActionResult> Edit(int? id, string izbranTipIzdelka, string SearchString, string filtriranje, int? stevilkaStrani)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izdelek = await _context.Izdelki.FindAsync(id);
            if (izdelek == null)
            {
                return NotFound();
            }

            ViewData["TipIzdelka"] = izbranTipIzdelka;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["SearchString"] = SearchString;
            ViewData["StevilkaStrani"] = stevilkaStrani;

            ViewData["MaterialID"] = new SelectList(_context.Materiali, "ID", "Ime");
            ViewData["TipIzdelkaID"] = new SelectList(_context.TipiIzdelkov, "ID", "Ime");
            var novaCena = Convert.ToDecimal(izdelek.Cena);
            ViewData["Cena"] = novaCena;
            return View(izdelek);
        }

        // POST: Izdelki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ime,Opis,Podrobnosti,Cena,Izbrisan,Dodan,Posodobljen, PovprecnaOcena, MaterialID,TipIzdelkaID")] Izdelek izdelek)
        {
            if (id != izdelek.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    izdelek.Posodobljen = DateTime.Now;
                    _context.Update(izdelek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IzdelekExists(izdelek.ID))
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
            ViewData["MaterialID"] = new SelectList(_context.Materiali, "ID", "ID", izdelek.MaterialID);
            ViewData["TipIzdelkaID"] = new SelectList(_context.TipiIzdelkov, "ID", "ID", izdelek.TipIzdelkaID);

           
            return View(izdelek);
        }

        // GET: Izdelki/Delete/5
        public async Task<IActionResult> Delete(int? id,  string izbranTipIzdelka, string SearchString, string filtriranje, int? stevilkaStrani, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izdelek = await _context.Izdelki
                .Include(i => i.Material)
                .Include(i => i.TipIzdelka)
                .Include(i => i.Opcije)
                .ThenInclude(i => i.TipOpcije)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (izdelek == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            ViewData["TipIzdelka"] = izbranTipIzdelka;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["SearchString"] = SearchString;
            ViewData["StevilkaStrani"] = stevilkaStrani;

            return View(izdelek);
        }

        // POST: Izdelki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var izdelek = await _context.Izdelki.FindAsync(id);
            if (izdelek == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                izdelek.Izbrisan = true;
                _context.Izdelki.Update(izdelek);
                //_context.Izdelki.Remove(izdelek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recover(int id)
        {
            var izdelek = await _context.Izdelki.FindAsync(id);
            if (izdelek == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                izdelek.Izbrisan = false;
                _context.Izdelki.Update(izdelek);
                //_context.Izdelki.Remove(izdelek);
                await _context.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                
                return RedirectToAction(nameof(Index));
            }
        }

        private bool IzdelekExists(int id)
        {
            return _context.Izdelki.Any(e => e.ID == id);
        }
    }
}
