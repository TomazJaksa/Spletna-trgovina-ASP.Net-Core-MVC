using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Voleska.Areas.Identity.Data;
using Voleska.Data;
using Voleska.Models;

namespace Voleska.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class UporabnikiController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public UporabnikiController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Uporabniki
        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, string searchString4, int? stevilkaStrani, int? kliknjenLinkStrani, string filtriranje)
        {
            var uporabniki = from m in _context.Users
                        select m;


            if (searchString != null || searchString2 != null || searchString3 != null || searchString4 != null || filtriranje != null)
            {
                stevilkaStrani = 1;

                if (kliknjenLinkStrani != null)
                {
                    stevilkaStrani = kliknjenLinkStrani;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    uporabniki = uporabniki.Where(s => s.UserName.Contains(searchString));

                }
                if (!String.IsNullOrEmpty(searchString2))
                {
                    uporabniki = uporabniki.Where(s => s.Email.Contains(searchString2));

                }
                if (!String.IsNullOrEmpty(searchString3))
                {
                    uporabniki = uporabniki.Where(s => s.Ime.Contains(searchString3));

                }
                if (!String.IsNullOrEmpty(searchString4))
                {
                    uporabniki = uporabniki.Where(s => s.Priimek.Contains(searchString4));

                }

                switch (filtriranje)
                {
                    case "datumPadajoce":
                        uporabniki = uporabniki.OrderByDescending(s => s.Dodan);
                        ViewData["IzbranFilter"] = "Najnovejši";
                        break;
                    case "datumNarascajoce":
                        uporabniki = uporabniki.OrderBy(s => s.Dodan);
                        ViewData["IzbranFilter"] = "Najstarejši";
                        break;
                }


            }


            ViewData["Filtriranje"] = filtriranje;

            if (stevilkaStrani == null)
            {
                ViewData["StevilkaStrani"] = 1;
            }
            else
            {
                ViewData["StevilkaStrani"] = stevilkaStrani;
            }

            List<ApplicationUser> ustrezniUporabniki = await uporabniki.Include(u => u.UserRoles).ThenInclude(u=>u.ApplicationRole).Include(u => u.Naslov).ThenInclude(p => p.Posta).ToListAsync();
            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["SearchString4"] = searchString4;

            int steviloPrikazanihUporabnikov = 3;

            return View(PaginatedList<ApplicationUser>.CreateAsync(ustrezniUporabniki, stevilkaStrani ?? 1, steviloPrikazanihUporabnikov));
        }

        // GET: Uporabniki/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Users
                .Include(u => u.Naslov)
                .ThenInclude(u => u.Posta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uporabnik == null)
            {
                return NotFound();
            }

            return View(uporabnik);
        }

        // GET: Uporabniki/Create
        public IActionResult Create()
        {

            ViewData["NaslovID"] = new SelectList(_context.Naslovi, "ID", "ID");
            return View();
        }

        // POST: Uporabniki/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,PasswordHash,Email,Ime,Priimek,Aktiven")] ApplicationUser uporabnikArg, string hisnaStevilka)
        {
            
            if (ModelState.IsValid)
            {
                ApplicationUser uporabnik = new ApplicationUser();
                Naslov naslov = new Naslov();
                Posta posta = new Posta();

                
                posta.PostnaStevilka = "";
                posta.Kraj = "";
                posta.Dodan = DateTime.Now;
                posta.Posodobljen = DateTime.Now;
                _context.Add(posta);

                await _context.SaveChangesAsync();

                naslov.Ulica = "";
                naslov.HisnaStevilka = hisnaStevilka;
                naslov.Dodan = DateTime.Now;
                naslov.Posodobljen = DateTime.Now;
                naslov.PostaID = posta.ID;
                _context.Add(naslov);
                await _context.SaveChangesAsync();

                uporabnik.UserName = uporabnikArg.UserName;
                uporabnik.PasswordHash = uporabnikArg.PasswordHash;
                uporabnik.Email = uporabnikArg.Email;
                uporabnik.Ime = uporabnikArg.Ime;
                uporabnik.Priimek = uporabnikArg.Priimek;
                uporabnik.Aktiven = uporabnikArg.Aktiven;
                uporabnik.Dodan = DateTime.Now;
                uporabnik.Posodobljen = DateTime.Now;
                uporabnik.NaslovID = naslov.ID;
                _context.Add(uporabnik);
                
                
                
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NaslovID"] = new SelectList(_context.Naslovi, "ID", "ID", uporabnikArg.NaslovID);
            return View(uporabnikArg);
        }

        // GET: Uporabniki/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Users.FindAsync(id);
            if (uporabnik == null)
            {
                return NotFound();
            }
            ViewData["NaslovID"] = new SelectList(_context.Naslovi, "ID", "ID", uporabnik.NaslovID);
            return View(uporabnik);
        }

        // POST: Uporabniki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,PasswordHash,Email,Ime,Priimek,Aktiven,Dodan,Posodobljen,NaslovID")] ApplicationUser uporabnik)
        {
            if (id != uporabnik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uporabnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UporabnikExists(uporabnik.Id))
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
            ViewData["NaslovID"] = new SelectList(_context.Naslovi, "ID", "ID", uporabnik.NaslovID);
            return View(uporabnik);
        }

        // GET: Uporabniki/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Users
                .Include(u => u.Naslov)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uporabnik == null)
            {
                return NotFound();
            }

            return View(uporabnik);
        }

        // POST: Uporabniki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uporabnik = await _context.Users.FindAsync(id);
            _context.Users.Remove(uporabnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UporabnikExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
