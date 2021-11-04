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

            int steviloPrikazanihUporabnikov = 5;

            return View(PaginatedList<ApplicationUser>.CreateAsync(ustrezniUporabniki, stevilkaStrani ?? 1, steviloPrikazanihUporabnikov));
        }

        // GET: Uporabniki/Details/5
        public async Task<IActionResult> Details(string id, string searchString, string searchString2, string searchString3, string searchString4, int? stevilkaStrani,  string filtriranje)
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

            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["SearchString4"] = searchString4;
            ViewData["StevilkaStrani"] = stevilkaStrani;
            ViewData["Filtriranje"] = filtriranje;

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
        public async Task<IActionResult> Edit(string id, string searchString, string searchString2, string searchString3, string searchString4, int? stevilkaStrani, string filtriranje)
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
            ViewData["NaslovID"] = new SelectList(_context.Naslovi, "ID", "ID", uporabnik.NaslovID);
            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["SearchString4"] = searchString4;
            ViewData["StevilkaStrani"] = stevilkaStrani;
            ViewData["Filtriranje"] = filtriranje;
            return View(uporabnik);
        }

        // POST: Uporabniki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,string username, string email, string ime, string priimek, bool aktiven, int naslovId, string ulica, string hisnaStevilka, string postnaStevilka, string kraj )
        {
            var uporabnik = await _context.Users.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    uporabnik.Ime = ime;
                    uporabnik.Priimek = priimek;
                    uporabnik.Aktiven = aktiven;
                    uporabnik.Posodobljen = DateTime.Now;

                    _context.Update(uporabnik);
                    await _context.SaveChangesAsync();

                    
                    var naslov = await _context.Naslovi.FindAsync(uporabnik.NaslovID);
                    naslov.Ulica = ulica;
                    naslov.HisnaStevilka = hisnaStevilka;

                    _context.Update(naslov);
                    await _context.SaveChangesAsync();


                    var posta = await _context.Poste.FindAsync(naslov.PostaID);
                    posta.PostnaStevilka = postnaStevilka;
                    posta.Kraj = kraj;

                    _context.Update(posta);
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
        public async Task<IActionResult> Delete(string id, string searchString, string searchString2, string searchString3, string searchString4, int? stevilkaStrani, string filtriranje)
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

            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["SearchString4"] = searchString4;
            ViewData["StevilkaStrani"] = stevilkaStrani;
            ViewData["Filtriranje"] = filtriranje;
            return View(uporabnik);
        }

        // POST: Uporabniki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var uporabnik = await _context.Users.FindAsync(id);
            var naslov = await _context.Naslovi.FindAsync(uporabnik.NaslovID);
            var posta = await _context.Poste.FindAsync(naslov.PostaID);


            var lajkKomentarjev = from m in _context.LajkaniKomentarji
                                  select m;

            lajkKomentarjev = lajkKomentarjev.Where(s => s.ApplicationUserID == id);

            foreach (var lajk in lajkKomentarjev)
            {
                _context.LajkaniKomentarji.Remove(lajk);
                var komentar = _context.Komentarji.FirstOrDefault(u => u.ID == lajk.KomentarID);
                if (lajk.Lajk == true)
                {
                    komentar.SteviloLike -= 1;
                    _context.Komentarji.Update(komentar);
                }
                else
                {
                    komentar.SteviloDislike -= 1;
                    _context.Komentarji.Update(komentar);
                }
            }
            await _context.SaveChangesAsync();


            var komentarji = from m in _context.Komentarji
                              select m;

            komentarji = komentarji.Where(s => s.ApplicationUserID == id);

            foreach (var komentar in komentarji) {
                //_context.Komentarji.Remove(komentar);
                IzbrisiKomentar(komentar.ID);
            }
            await _context.SaveChangesAsync();

           

            

            var lajkBlogov = from m in _context.LajkaniBlogi
                                  select m;

            lajkBlogov = lajkBlogov.Where(s => s.ApplicationUserID == id);

            foreach (var lajk in lajkBlogov)
            {
                _context.LajkaniBlogi.Remove(lajk);
                var blog = _context.Blogi.FirstOrDefault(u => u.ID == lajk.BlogID);
                if (lajk.Lajk == true)
                {
                    blog.SteviloLike -= 1;
                    _context.Blogi.Update(blog);
                }
                else
                {
                    blog.SteviloDislike -= 1;
                    _context.Blogi.Update(blog);
                }
            }
            await _context.SaveChangesAsync();


            var oceneIzdelkov = from m in _context.OceneIzdelkov
                             select m;

            oceneIzdelkov = oceneIzdelkov.Where(s => s.ApplicationUserID == id);

            foreach (var ocena in oceneIzdelkov)
            {
                _context.OceneIzdelkov.Remove(ocena);
            }
            await _context.SaveChangesAsync();


            var transakcije = from m in _context.Transakcije
                                select m;

            transakcije = transakcije.Where(s => s.ApplicationUserID == id);

            foreach (var transakcija in transakcije)
            {
                _context.Transakcije.Remove(transakcija);
            }
            await _context.SaveChangesAsync();


            var novice = from m in _context.Novice
                                select m;

            novice = novice.Where(s => s.Email == uporabnik.Email);

            foreach (var narocilo in novice)
            {
                _context.Novice.Remove(narocilo);
            }
            await _context.SaveChangesAsync();


            _context.Naslovi.Remove(naslov);
            await _context.SaveChangesAsync();

            _context.Poste.Remove(posta);
            await _context.SaveChangesAsync();
            /*
            _context.Users.Remove(uporabnik);
            await _context.SaveChangesAsync();
            */
            return RedirectToAction(nameof(Index));
        }

       

        private void IzbrisiKomentar(int id)
        {
            //Pobrišimo odgovore na ta komentar
            var odgovoriOdKomentarja = from m in _context.Odgovori select m;

            var taKomentarKotOdgovor = odgovoriOdKomentarja.Where(s => s.OdgovorNaTaKomentarID == id);

            odgovoriOdKomentarja = odgovoriOdKomentarja.Where(s => s.TaKomentarID == id);

            if (taKomentarKotOdgovor != null)
            {
                foreach (var odgovor in taKomentarKotOdgovor)
                {
                    _context.Odgovori.Remove(odgovor);
                }
            }
             //_context.SaveChanges();

            //odgovori na ta odgovor
            List<Komentar> seznamOdgovorov = new List<Komentar>();

            if (odgovoriOdKomentarja != null)
            {
                foreach (var odgovor in odgovoriOdKomentarja)
                {
                    var najdiKomentar = _context.Komentarji.Find(odgovor.OdgovorNaTaKomentarID);

                    seznamOdgovorov.Add(najdiKomentar);

                    _context.Odgovori.Remove(odgovor);
                }
            }

            //_context.SaveChanges();

            //Sedaj se lotimo brisanja komentarja
            var komentar =  _context.Komentarji.Find(id);

            var lajkiOdKomentarja = from m in _context.LajkaniKomentarji
                                    select m;

            lajkiOdKomentarja = lajkiOdKomentarja.Where(s => s.KomentarID == id);

            foreach (var lajk in lajkiOdKomentarja)
            {
                _context.LajkaniKomentarji.Remove(lajk);
            }
            //_context.SaveChanges();

            _context.Komentarji.Remove(komentar);
            //_context.SaveChanges();


            if (seznamOdgovorov.Count() > 0)
            {
                foreach (var kom in seznamOdgovorov)
                {
                    PocistiOdgovore(kom.ID);
                }
            }






        }

        private void PocistiOdgovore(int komentarId)
        {
            //Pobrišimo odgovore na ta komentar
            var odgovoriOdKomentarja = from m in _context.Odgovori select m;

            odgovoriOdKomentarja = odgovoriOdKomentarja.Where(s => s.TaKomentarID == komentarId);


            //odgovori na ta odgovor
            List<Komentar> seznamOdgovorov = new List<Komentar>();

            if (odgovoriOdKomentarja != null)
            {
                foreach (var odgovor in odgovoriOdKomentarja)
                {
                    var najdiKomentar = _context.Komentarji.Find(odgovor.OdgovorNaTaKomentarID);

                    seznamOdgovorov.Add(najdiKomentar);


                    _context.Odgovori.Remove(odgovor);



                }
            }

           // _context.SaveChanges();

            var lajkiOdKomentarja = from m in _context.LajkaniKomentarji
                                    select m;

            lajkiOdKomentarja = lajkiOdKomentarja.Where(s => s.KomentarID == komentarId);

            foreach (var lajk in lajkiOdKomentarja)
            {
                _context.LajkaniKomentarji.Remove(lajk);
            }
           // _context.SaveChanges();


            var komentar = _context.Komentarji.Find(komentarId);

            _context.Komentarji.Remove(komentar);

            //_context.SaveChanges();



            if (seznamOdgovorov.Count() > 0)
            {
                foreach (var kom in seznamOdgovorov)
                {
                    PocistiOdgovore(kom.ID);
                }
            }
        }

        private bool UporabnikExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
