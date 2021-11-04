using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voleska.Areas.Identity.Data;
using Voleska.Models;
using Voleska.Services;
using Voleska.ViewModel;

namespace Voleska.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class SporocilaController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public SporocilaController(Data.ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }


        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, int? stevilkaStrani, int? kliknjenLinkStrani, string filtriranje)
        {
            var vir = from m in _context.Dopisovanja
                              select m;
            vir = vir.Include(x => x.Sporocilo).Include(y => y.Pogovor);

            vir = vir.OrderByDescending(s => s.Dodan);

            if (searchString != null || searchString2 != null || searchString3 != null || filtriranje != null)
            {
                //stevilkaStrani = 1;

                if (kliknjenLinkStrani != null)
                {
                    stevilkaStrani = kliknjenLinkStrani;
                }


                if (!String.IsNullOrEmpty(searchString))
                {
                    vir = vir.Where(s => s.Sporocilo.Email.Contains(searchString));

                }
                if (!String.IsNullOrEmpty(searchString2))
                {
                    vir = vir.Where(s => s.Sporocilo.Ime.Contains(searchString2));

                }
                if (!String.IsNullOrEmpty(searchString3))
                {
                    vir = vir.Where(s => s.Sporocilo.Priimek.Contains(searchString3));

                }

               
               
                switch (filtriranje)
                {
                    case "datumPadajoce":
                        vir = vir.OrderByDescending(s => s.Dodan);
                        ViewData["IzbranFilter"] = "Najnovejše";
                        break;
                    case "datumNarascajoce":
                        vir = vir.OrderBy(s => s.Dodan);
                        ViewData["IzbranFilter"] = "Najstarejše";
                        break;


                }
                

                


            }


            vir = vir.ToList().AsQueryable();

            foreach (var vrstica in vir) {
                if (vrstica.Sporocilo.Odprto==false) {
                    vrstica.Pogovor.Izbrisan = false;
                    _context.Update(vrstica);
                }
            }
            await _context.SaveChangesAsync();


           


            var items = vir.Where(x => x.Pogovor.Izbrisan == false)
                .GroupBy(u => u.PogovorID)
                .ToList();



           


            InboxViewModel model = new InboxViewModel();
            List<SporociloViewModel> sporocila = new List<SporociloViewModel>();
            
            if (items.Count()==0) {
                ViewData["PrazenSeznam"] = true;
            }

            foreach (var id in items) {
                SporociloViewModel s = new SporociloViewModel();

                var praviDatum = id.Max(x => x.Dodan);

                

                s.PogovorID = id.Key;
                
                s.Dodan = praviDatum;


                foreach (var element in id)
                {
                   
                    if (element.Dodan == praviDatum && element != null) {
                        s.Sporocilo = element.Sporocilo;
                    }

                    if (element.Sporocilo.Ime != "Administrator") {
                        s.Ime = element.Sporocilo.Ime + " " + element.Sporocilo.Priimek + " (" + element.Sporocilo.Email + ")"; 
                    }
                    
                }

                sporocila.Add(s);
            }

            //model.Sporocila = sporocila;


            // return View(model);
            if (stevilkaStrani == null)
            {
                ViewData["StevilkaStrani"] = 1;
            }
            else
            {
                ViewData["StevilkaStrani"] = stevilkaStrani;
            }

            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["Filtriranje"] = filtriranje;



            int steviloPrikazanihSporocil = 10;

            return View(PaginatedList<SporociloViewModel>.CreateAsync(sporocila, stevilkaStrani ?? 1, steviloPrikazanihSporocil));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(string vsebina, bool admin, int pogovorId)
        {
            Sporocilo sporocilo = new Sporocilo();
            bool administrator = admin;

            Dopisovanje novaVrstica = new Dopisovanje();

            var preveriPogovor = await _context.Pogovori
                .FirstOrDefaultAsync(m => m.ID == pogovorId);

            if (preveriPogovor.Izbrisan == true) {
                preveriPogovor.Izbrisan = false;
                _context.Update(preveriPogovor);
                await _context.SaveChangesAsync();
            }

            if (ModelState.IsValid)
            {
                

                var sporocila = from m in _context.Dopisovanja
                                select m;

                sporocila = sporocila.Where(s => s.PogovorID == pogovorId);
                sporocila = sporocila.OrderByDescending(s => s.Dodan);
                sporocila = sporocila.Include(x => x.Sporocilo);

                var setIme = "";
                var setPriimek = "";
                var setEmail = "";
                var setTema = "";
                foreach (var item in sporocila)
                {
                    if (sporocila.First() == item)
                    {

                          item.Sporocilo.Odgovorjeno = true;
                        
                    }

                    setTema = item.Sporocilo.Tema;
                    if (item.Sporocilo.Email != "voleska.info@gmail.com")
                    {
                        setIme = item.Sporocilo.Ime;
                        setPriimek = item.Sporocilo.Priimek;
                        setEmail = item.Sporocilo.Email;
                        break;
                    }

                }

           

                if (administrator == true) {

                    sporocilo.Ime = "Administrator";
                    sporocilo.Priimek = null;
                    sporocilo.Email = "voleska.info@gmail.com";
                    sporocilo.Odprto = true;
                    sporocilo.Odgovorjeno = true;
                }
                else{
                    sporocilo.Ime = setIme;
                    sporocilo.Priimek = setPriimek;
                    sporocilo.Email = setEmail;
                    sporocilo.Odprto = false;
                    sporocilo.Odgovorjeno = false;
                }
                
                sporocilo.Tema = setTema;
                sporocilo.Vsebina = vsebina;
                


                _context.Add(sporocilo);
                await _context.SaveChangesAsync();

                novaVrstica.PogovorID = pogovorId;
                novaVrstica.SporociloID = sporocilo.ID;
                novaVrstica.Dodan = DateTime.Now;

                _context.Add(novaVrstica);
                await _context.SaveChangesAsync();

                var sendGridName = "Voleska info";
                var sendGrid = "voleska.info@gmail.com";
                var naslovnik = "";
                var naslovnikIme = "";
                var senderName = "";
                var senderSurname = "";
                var subject = sporocilo.Tema;
                var message =sporocilo.Vsebina;

                if (administrator == false)
                {
                    naslovnik = "jaksa.tomaz@gmail.com";
                    naslovnikIme = "T.J.";
                    senderName = setIme;
                    senderSurname = setPriimek;
                    await _mailService.SendEmailAsync(sendGridName, sendGrid, naslovnik, naslovnikIme, senderName, senderSurname, subject, message, pogovorId);


                }
                else
                {
                    naslovnik = setEmail;
                    naslovnikIme = setIme + " " + setPriimek;
                    senderName = "Administrator";
                    senderSurname = "";
                    await _mailService.SendEmailAsync(sendGridName, sendGrid, naslovnik, naslovnikIme, senderName, senderSurname, subject, message, pogovorId);

                }
              
                var sporociloId = sporocilo.ID;

                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovKomentarVC), new { komentarId = sporociloId });
               

            }
           
            return RedirectToAction(nameof(Index));

        }

        // GET: Sporocila/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id, bool admin, string searchString, string searchString2, string searchString3, int? stevilkaStrani, string filtriranje)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pogovor = from m in _context.Dopisovanja
                              select m;

           

             pogovor =  pogovor.Where(m => m.PogovorID == id).OrderBy(s => s.Dodan);

            

            if (pogovor == null)
            {
                return NotFound();
            }

           var model=  await pogovor.Include(x => x.Sporocilo).ToListAsync();

            if (admin==true) {
                foreach (var sporocilo in model)
                {
                    sporocilo.Sporocilo.Odprto = true;
                    _context.Update(sporocilo);
                    await _context.SaveChangesAsync();
                }
            }


            foreach (var sporocilo in model)
            {
                if (sporocilo.Sporocilo.Tema != null)
                {
                    ViewData["Tema"] = sporocilo.Sporocilo.Tema;
                    break;
                }

            }
            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["StevilkaStrani"] = stevilkaStrani;

            ViewData["PogovorID"] = id;
            return View(model);
        }

        // GET: Sporocila/Delete/5
        public async Task<IActionResult> Delete(int? id, string searchString, string searchString2, string searchString3, int? stevilkaStrani, string filtriranje)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pogovor = from m in _context.Dopisovanja
                          select m;



            pogovor = pogovor.Where(m => m.PogovorID == id).OrderBy(s => s.Dodan);



            if (pogovor == null)
            {
                return NotFound();
            }

            var model = await pogovor.Include(x => x.Sporocilo).ToListAsync();

            foreach (var sporocilo in model)
            {
                if (model.Last() == sporocilo) {
                    sporocilo.Sporocilo.Odprto = true;
                    _context.Update(sporocilo);
                    
                } 

            }

            await _context.SaveChangesAsync();

            foreach (var sporocilo in model)
            {
                if (sporocilo.Sporocilo.Tema != null)
                {
                    ViewData["Tema"] = sporocilo.Sporocilo.Tema;
                    break;
                }

            }
            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["StevilkaStrani"] = stevilkaStrani;

            ViewData["PogovorID"] = id;
            return View(model);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var pogovor = await _context.Pogovori.FindAsync(id);
            pogovor.Izbrisan = true;

            _context.Pogovori.Update(pogovor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult SeznamReload(int sporociloId, int pogovorId)
        {
            

            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.SeznamSporocilVC), new { sporociloId = sporociloId, pogovorId = pogovorId });
        }

        [AllowAnonymous]
        public IActionResult VnosReload()
        {


            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.VnosKomentarjaVC), new { tip = "sporocilo" });
        }


        public IActionResult NovOglas(string searchString, string searchString2, string searchString3, int? stevilkaStrani, string filtriranje)
        {
            

       
            ViewData["SearchString"] = searchString;
            ViewData["SearchString2"] = searchString2;
            ViewData["SearchString3"] = searchString3;
            ViewData["Filtriranje"] = filtriranje;
            ViewData["StevilkaStrani"] = stevilkaStrani;

            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> NovOglas(string naslov, string vsebina)
        {
            if (ModelState.IsValid)
            {

                var narocniki = from m in _context.Novice
                              select m;

                var users = from m in _context.Users
                                select m;

                var admin = "jaksa.tomaz@gmail.com";
                var adminIme = "T.J.";
                var sendGridIme = "Voleska info";
                var sendGridEmail = "voleska.info@gmail.com";

                IDictionary<string, string> vsebinaMail = new Dictionary<string, string>();
                vsebinaMail.Add("naslov", naslov); //adding a key/value using the Add() method
                vsebinaMail.Add("vsebina", vsebina);

                foreach (var narocnik in narocniki) {
                    ApplicationUser naslovnik = null;
                    foreach (var user in users) {
                        if (user.Email==narocnik.Email) {
                            naslovnik = user;
                            break;
                        }
                    }
                   
                    var naslovnikIme = naslovnik.Ime + " " + naslovnik.Priimek;



                    await _mailService.SendEmailAsync(sendGridIme, sendGridEmail, naslovnik.Email, naslovnikIme, vsebinaMail);

                }

                await _mailService.SendEmailAsync(sendGridIme, sendGridEmail, admin, adminIme, vsebinaMail);


                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        


    }
}
