using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using Voleska.Models;
using Voleska.Services;
using Voleska.ViewModel;

namespace Voleska.Controllers
{
    [AllowAnonymous]
    public class KatalogController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public KatalogController(Data.ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;

        }

        // GET: Izberi tip izdelka
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.TipiIzdelkov;
            return View(await storeContext.ToListAsync());
        }

        public async Task<IActionResult> Izdelki(int id, string imeTipaIzdelka, string searchString, string filtriranje, int? stevilkaStrani, int? kliknjenLinkStrani) {

            // Use LINQ to get list of genres.
            IQueryable<int> genreQuery = from m in _context.Izdelki
                                            orderby m.TipIzdelkaID
                                            select m.TipIzdelkaID;

            var izdelki = from m in _context.Izdelki
                         select m;

                izdelki = izdelki.Where(x => x.TipIzdelkaID == id);

            if (searchString != null || filtriranje != null)
            {
                stevilkaStrani = 1;

                if (kliknjenLinkStrani != null)
                {
                    stevilkaStrani = kliknjenLinkStrani;
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    izdelki = izdelki.Where(s => s.Ime.Contains(searchString));
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

            ViewData["Filtriranje"] = filtriranje;
            if (stevilkaStrani == null)
            {
                ViewData["StevilkaStrani"] = 1;
            }
            else
            {
                ViewData["StevilkaStrani"] = stevilkaStrani;
            }

            ViewData["TipIzdelka"] = imeTipaIzdelka;
            List<Izdelek> izbraniIzdelki = await izdelki.Include(i => i.Material).Include(i => i.TipIzdelka).Include(i => i.Opcije).ToListAsync();

            ViewData["SearchString"] = searchString;

            int steviloPrikazanihIzdelkov = 6;

            return View(PaginatedList<Izdelek>.CreateAsync(izbraniIzdelki, stevilkaStrani ?? 1, steviloPrikazanihIzdelkov));
        }

        // GET: Izdelki/Details/5
        public async Task<IActionResult> Details(int? id, string imeTipaIzdelka, bool naKomentarje, string searchString, string filtriranje, int? stevilkaStrani)
        {
            if (naKomentarje==true) {
                TempData["TipIzdelka"] = imeTipaIzdelka;
                return RedirectToAction("Details", "Katalog", new { id = id }, "ocene-in-komentarji");

            }

            if (id == null)
            {
                return NotFound();
            }

            var izdelek = await _context.Izdelki
                .Include(i => i.Material)
                .Include(i => i.Opcije)
                .Include(i => i.Narocila)
                .Include(i => i.OceneIzdelkov)
                    .ThenInclude(i => i.ApplicationUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (izdelek == null)
            {
                return NotFound();
            }

            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var obstojTransakcije = await _context.Transakcije
                    .FromSqlRaw(@"SELECT * FROM Transakcija WHERE  ApplicationUserID = {0} AND Zakljuceno= {1}", user, true)
                    .AsNoTracking()
                    .ToListAsync();

            
            var oceneIzdelkov = from m in _context.OceneIzdelkov
                           select m;

            var oceneIzdelka = oceneIzdelkov.Where(x => x.IzdelekID == id).ToList();
            

            ViewData["SteviloOcen"] = oceneIzdelka.Count();
            ViewData["OceneIzdelka"] = oceneIzdelka;
            ViewData["TransakcijeUporabnika"] = obstojTransakcije;
            ViewData["TrenutniUser"] = user;
            ViewData["TipIzdelka"] = imeTipaIzdelka;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["StevilkaStrani"] = stevilkaStrani;
            ViewData["SearchString"] = searchString;

            var opcije = from m in _context.Opcije select m;
            opcije = opcije.Where(x => x.IzdelekID == id);

            

            ViewData["Opcije"] = new SelectList(opcije, "ID", "Ime");
            ViewData["MaterialID"] = new SelectList(_context.Materiali, "ID", "Ime");

            return View(izdelek);
        }

        // POST: DodajVKosarico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajVKosarico(int izdelekId,string opcijaParameter, int MaterialID, int kolicina, decimal cena, string opombe, int tipIzdelkaId, string imeTipaIzdelka, string searchString, string filtriranje, int? stevilkaStrani)
        {

            
            
            if (ModelState.IsValid)
            {
                var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);


                var obstojTransakcije = await _context.Transakcije
                    .FromSqlRaw(@"SELECT * FROM Transakcija WHERE  ApplicationUserID = {0} AND Zakljuceno= {1}", user, false)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                var opcija = await _context.Opcije
                    .FromSqlRaw(@"SELECT * FROM Opcija WHERE  Slika = {0}", opcijaParameter)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();


                if (obstojTransakcije == null)
                {
                    Transakcija transakcija = new Transakcija();
                    transakcija.Dodan = DateTime.Now;
                    transakcija.Posodobljen = DateTime.Now;
                    transakcija.Zakljuceno = false;
                    transakcija.Odposlano = false;
                    transakcija.ApplicationUserID = user;

                    _context.Add(transakcija);
                    await _context.SaveChangesAsync();

                    Narocilo narocilo = new Narocilo();


                    narocilo.Kolicina = kolicina;
                    narocilo.Opombe = opombe;
                    narocilo.Znesek = kolicina * cena;
                    narocilo.Dodan = DateTime.Now;
                    narocilo.Posodobljen = DateTime.Now;
                    narocilo.IzdelekID = izdelekId;
                    narocilo.TransakcijaID = transakcija.ID;
                    
                    _context.Add(narocilo);
                    await _context.SaveChangesAsync();


                    var rowsAffected = _context.Database
                      .ExecuteSqlRaw(
                          @"UPDATE Narocilo
                            SET OpcijaID = {0}, MaterialID = {1}                     
                            WHERE ID = {2}", opcija.ID, MaterialID, narocilo.ID);

                    transakcija.SkupniZnesek = narocilo.Znesek;
                    _context.Update(transakcija);
                    await _context.SaveChangesAsync();

                }
                else
                {

                    var obstojNarocila = await _context.Narocila
                    .FromSqlRaw(@"SELECT * FROM Narocilo WHERE  TransakcijaID = {0} AND IzdelekID = {1} AND OpcijaID = {2} AND MaterialID= {3}", obstojTransakcije.ID, izdelekId, opcija.ID, MaterialID)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                    if (obstojNarocila == null)
                    {
                        Narocilo narocilo = new Narocilo();

                        narocilo.Kolicina = kolicina;
                        narocilo.Znesek = kolicina * cena;
                        narocilo.Dodan = DateTime.Now;
                        narocilo.Posodobljen = DateTime.Now;
                        narocilo.IzdelekID = izdelekId;
                        narocilo.TransakcijaID = obstojTransakcije.ID;

                        narocilo.Opombe = opombe;

                        _context.Add(narocilo);
                        await _context.SaveChangesAsync();


                        var rowsAffected = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Narocilo
                            SET OpcijaID = {0}, MaterialID = {1}                     
                            WHERE ID = {2}", opcija.ID, MaterialID, narocilo.ID);

                        obstojTransakcije.SkupniZnesek += narocilo.Znesek;
                        _context.Update(obstojTransakcije);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        obstojNarocila.Opombe = opombe;

                        _context.Update(obstojNarocila);
                        await _context.SaveChangesAsync();

                        int novaKolicina = obstojNarocila.Kolicina + kolicina;
                        decimal decimalKolicina = kolicina;
                        decimal noviZnesek = obstojNarocila.Znesek + (decimalKolicina * cena);

                        var increase = _context.Database
                           .ExecuteSqlRaw(
                               @"UPDATE Narocilo
                            SET Kolicina = {0}, Znesek = {1}, Posodobljen = {2}                      
                            WHERE ID = {3}", novaKolicina, noviZnesek, DateTime.Now, obstojNarocila.ID);

                        obstojTransakcije.SkupniZnesek += decimalKolicina*cena;
                        _context.Update(obstojTransakcije);
                    }

                }
                
                

                await _context.SaveChangesAsync();
                return RedirectToAction("Izdelki","Katalog", new { id = tipIzdelkaId, imeTipaIzdelka=imeTipaIzdelka, searchString=searchString, filtriranje=filtriranje , stevilkaStrani=stevilkaStrani });
                /*return RedirectToAction("Details", "Blogi", new { @id = blogId });*/

            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PoglejKosarico(int transakcijaId)
        {
            var narocila = from m in _context.Narocila
                          select m;

           var ustreznaNarocila = narocila.Where(x => x.TransakcijaID == transakcijaId);

            List<Narocilo> narocilaVpisanegaUporabnika = await ustreznaNarocila.Include(i => i.Material).Include(i => i.Izdelek).Include(i => i.Opcija).Include(i => i.Transakcija).ToListAsync();


            return View(narocilaVpisanegaUporabnika);
        }

        // POST: Izdelki/Delete/5
        [HttpPost]
        
        public async Task<IActionResult> OdstraniIzKosarice(int narociloId)
        {
            var narocilo = await _context.Narocila.FindAsync(narociloId);
            if (narocilo == null)
            {
                return RedirectToAction(nameof(PoglejKosarico));
            }

            try
            {
                var posodobiSkupniZnesekTransakcije = await _context.Transakcije.FindAsync(narocilo.TransakcijaID);
                posodobiSkupniZnesekTransakcije.SkupniZnesek -= narocilo.Znesek;
                _context.Update(posodobiSkupniZnesekTransakcije);
                _context.Narocila.Remove(narocilo);
                await _context.SaveChangesAsync();



                var narocila = from m in _context.Narocila
                               select m;

                var ustreznaNarocila = narocila.Where(x => x.TransakcijaID == narocilo.TransakcijaID);

                var kolicina = narocilo.Kolicina;
                var znesek = narocilo.Znesek;
                var skupniZnesek = posodobiSkupniZnesekTransakcije.SkupniZnesek;
                var narociloAjax = narociloId;
                var skupnaKolicina = 0;

                foreach (var dobiKolicino in ustreznaNarocila)
                {
                    skupnaKolicina += dobiKolicino.Kolicina;
                }

                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkov2VC), new { kolicina = kolicina, znesek = znesek, skupniZnesek = skupniZnesek, skupnaKolicina = skupnaKolicina, narociloId = narociloAjax });

                /*return RedirectToAction(nameof(PoglejKosarico), new { transakcijaId = narocilo.TransakcijaID });*/
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(PoglejKosarico), new { transakcijaId = narocilo.TransakcijaID });
            }
        }

        public IActionResult KolicinaReload(int kolicina,int narociloId)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.KolicinaVC), new { kolicina = kolicina, narociloId = narociloId });
        }

        public IActionResult ZnesekReload(decimal znesek, int narociloId)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.ZnesekVC), new { znesek = znesek, narociloId = narociloId });
        }

        public IActionResult SkupnaKolicinaReload( int skupnaKolicina)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.SkupnaKolicinaVC), new { skupnaKolicina = skupnaKolicina });
        }

        public IActionResult SkupniZnesekReload(decimal skupniZnesek)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.SkupniZnesekVC), new { skupniZnesek = skupniZnesek });
        }

        public IActionResult NarocilaReload(int narociloId)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.NarociloVC), new { narociloId = narociloId });
        }


        [HttpPost]
        
        public async Task<IActionResult> ZmanjsajStevilo(int narociloId/*, int trenutnaKolicinaParameterMinus*/)
        {
            
            var narocilo = await _context.Narocila.FindAsync(narociloId);
            var izdelek = await _context.Izdelki.FindAsync(narocilo.IzdelekID);
            var transakcija = await _context.Transakcije.FindAsync(narocilo.TransakcijaID);

            if (narocilo == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                /*if (trenutnaKolicinaParameterMinus == 0){*/

                    narocilo.Kolicina -= 1;
                    /*
                    if (narocilo.Kolicina == 0)
                    {
                        await OdstraniIzKosarice(narociloId);
                        
                    }
                    else
                    {*/
                        narocilo.Znesek -= izdelek.Cena;

                        transakcija.SkupniZnesek -= izdelek.Cena;
                        _context.Update(narocilo);
                        _context.Update(transakcija);

                        await _context.SaveChangesAsync();

                /* }
              }
              else {


                  narocilo.Kolicina = trenutnaKolicinaParameterMinus;

                  narocilo.Kolicina -= 1;

                  if (narocilo.Kolicina == 0)
                  {
                      await OdstraniIzKosarice(narociloId);
                  }
                  else
                  {
                      transakcija.SkupniZnesek -= narocilo.Znesek;
                      narocilo.Znesek = izdelek.Cena * narocilo.Kolicina;

                      transakcija.SkupniZnesek += narocilo.Znesek;
                      _context.Update(narocilo);
                      _context.Update(transakcija);

                      await _context.SaveChangesAsync();

                  }

              }
             */
                var narocila = from m in _context.Narocila
                               select m;

                var ustreznaNarocila = narocila.Where(x => x.TransakcijaID == narocilo.TransakcijaID);

                var kolicina = narocilo.Kolicina;
                var znesek = narocilo.Znesek;
                var skupniZnesek = transakcija.SkupniZnesek;
                var narociloAjax = narociloId;
                var skupnaKolicina = 0;

                foreach (var dobiKolicino in ustreznaNarocila)
                {
                    skupnaKolicina += dobiKolicino.Kolicina;
                }

                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovVC), new { kolicina = kolicina, znesek = znesek, skupniZnesek = skupniZnesek, skupnaKolicina = skupnaKolicina, narociloId= narociloAjax });

            }
            
            return RedirectToAction(nameof(PoglejKosarico), new { transakcijaId = narocilo.TransakcijaID });
            
        }

        [HttpPost]
        
        public async Task<IActionResult> PovecajStevilo(int narociloId/*, int trenutnaKolicinaParameterPlus*/)
        {
            var narocilo = await _context.Narocila.FindAsync(narociloId);
            var izdelek = await _context.Izdelki.FindAsync(narocilo.IzdelekID);
            var transakcija = await _context.Transakcije.FindAsync(narocilo.TransakcijaID);
            if (narocilo == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                /*if (trenutnaKolicinaParameterPlus == 0){*/

                    narocilo.Kolicina += 1;
                    narocilo.Znesek += izdelek.Cena;

                    transakcija.SkupniZnesek += izdelek.Cena;

                /*
                }
                else {
                    transakcija.SkupniZnesek -= narocilo.Znesek;
                    narocilo.Kolicina = trenutnaKolicinaParameterPlus;
                    narocilo.Kolicina += 1;
                    narocilo.Znesek = izdelek.Cena*narocilo.Kolicina;

                    transakcija.SkupniZnesek += narocilo.Znesek;
                }
                */


                _context.Update(narocilo);
                _context.Update(transakcija);

                await _context.SaveChangesAsync();

                var narocila = from m in _context.Narocila
                               select m;

                var ustreznaNarocila = narocila.Where(x => x.TransakcijaID == narocilo.TransakcijaID);

                var kolicina = narocilo.Kolicina;
                var znesek = narocilo.Znesek;
                var skupniZnesek = transakcija.SkupniZnesek;
                var narociloAjax = narociloId;
                var skupnaKolicina = 0;

                foreach (var dobiKolicino in ustreznaNarocila)
                {
                    skupnaKolicina += dobiKolicino.Kolicina;
                }

                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovVC), new { kolicina = kolicina, znesek = znesek, skupniZnesek = skupniZnesek, skupnaKolicina = skupnaKolicina, narociloId = narociloAjax });

            }

            return RedirectToAction(nameof(PoglejKosarico), new { transakcijaId = narocilo.TransakcijaID });
        }

        [HttpPost]

        public async Task<IActionResult> DodajOpombo(string opombaModal, int narociloIdModal, string imeIzdelkaModal)
        {
            var narocilo = await _context.Narocila.FindAsync(narociloIdModal);
           
            if (narocilo == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                narocilo.Opombe = opombaModal;
                

           


                _context.Update(narocilo);
                
                await _context.SaveChangesAsync();


                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.OpombaVC), new { opomba = opombaModal, narociloId = narociloIdModal, imeIzdelka = imeIzdelkaModal });

            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public async Task<IActionResult> OdstraniOpombo( int narociloId)
        {
            var narocilo = await _context.Narocila.FindAsync(narociloId);

            if (narocilo == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                narocilo.Opombe = null;





                _context.Update(narocilo);

                await _context.SaveChangesAsync();


                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.NarociloVC), new { narociloId=narociloId});

            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PosodobiRocniVnos(int narociloId, int trenutnaKolicina)
        {
            var narocilo = await _context.Narocila.FindAsync(narociloId);
            var izdelek = await _context.Izdelki.FindAsync(narocilo.IzdelekID);
            var transakcija = await _context.Transakcije.FindAsync(narocilo.TransakcijaID);
            if (narocilo == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                transakcija.SkupniZnesek -= narocilo.Znesek;

                narocilo.Kolicina = trenutnaKolicina;
                narocilo.Znesek = izdelek.Cena * narocilo.Kolicina;

                transakcija.SkupniZnesek += narocilo.Znesek;

                _context.Update(narocilo);
                _context.Update(transakcija);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(PoglejKosarico), new { transakcijaId = narocilo.TransakcijaID });
            }

            return RedirectToAction(nameof(PoglejKosarico), new { transakcijaId = narocilo.TransakcijaID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NaBlagajno(int transakcijaId)
        {
            var narocila = from m in _context.Narocila
                           select m;

            var ustreznaNarocila = narocila.Where(x => x.TransakcijaID == transakcijaId);

            List<Narocilo> narocilaVpisanegaUporabnika = await ustreznaNarocila.Include(i => i.Material).Include(i => i.Izdelek).Include(i => i.Opcija).Include(i => i.Transakcija).ToListAsync();


            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);


            var naslov = from m in _context.Naslovi
                           select m;

            var ustrezenNaslov = naslov.Where(x => x.ApplicationUserID == userId);

            foreach (var n in ustrezenNaslov) {
                if (n.Ulica==null || n.HisnaStevilka==null) {

                    TempData["DodajNaslov"] = "Za opravljanje nakupov morate imeti dodan naslov!";
                    
                    return Redirect("~/Identity/Account/Manage");
                }
            }



            var uporabniki = from m in _context.Users
                           select m;

            var ustrezenUser = uporabniki.Where(x => x.Id == userId);

            foreach (var uporabnik in ustrezenUser) {
                ViewData["UserMail"] = uporabnik.Email;
            }


            ViewData["TransakcijaId"] = transakcijaId;

            return View(narocilaVpisanegaUporabnika);
        }

        
        public async Task<IActionResult> Charge(string stripeEmail, string stripeToken, decimal skupniZnesekCenti, int transakcijaId)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

        
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = Decimal.ToInt64(skupniZnesekCenti),
                Description = "Nakup izdelkov. Transakcija ID: "+transakcijaId+".",
                Currency = "eur",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail
               
            });

            var posodobiTransakcijo = await _context.Transakcije.FindAsync(transakcijaId);
            posodobiTransakcijo.Zakljuceno = true;
            posodobiTransakcijo.Posodobljen = DateTime.Now;
            _context.Update(posodobiTransakcijo);

            await _context.SaveChangesAsync();

            var narocila = from m in _context.Narocila
                           select m;

            var ustreznaNarocila = narocila.Where(x => x.TransakcijaID == transakcijaId);

            List<Narocilo> narocilaVpisanegaUporabnika = await ustreznaNarocila.ToListAsync();


            foreach (var narocilo in narocilaVpisanegaUporabnika) {
                var posodobiIzdelek = await _context.Izdelki.FindAsync(narocilo.IzdelekID);
                posodobiIzdelek.SteviloProdanih += narocilo.Kolicina;
                _context.Update(posodobiIzdelek);

                await _context.SaveChangesAsync();
            }
            


            if (charge.Status == "succeeded")
            {

                    //mail pošljemo adminu
                    var admin = "jaksa.tomaz@gmail.com";
                    var adminIme = "T.J.";
                    var sendGridIme = "Voleska info";
                    var sendGridEmail = "voleska.info@gmail.com";
                    await _mailService.SendEmailAsync(sendGridIme, sendGridEmail, admin, adminIme, "Administrator", "Obvestilo o uspešno opravljeni transakciji", transakcijaId);


                    //in stranki
                    var naslovnik = stripeEmail;
                    var userId = posodobiTransakcijo.ApplicationUserID;
                    
                    var user = await _context.Users.FindAsync(userId);

                    var naslovnikIme = user.Ime + " " + user.Priimek;
                    
                    await _mailService.SendEmailAsync(sendGridIme, sendGridEmail, naslovnik, naslovnikIme,"Uporabnik", "Obvestilo o uspešno opravljeni transakciji", transakcijaId);

                


                return View();
            }
            else {
                return View("Error");
            }
            
        }
        
    }
}
