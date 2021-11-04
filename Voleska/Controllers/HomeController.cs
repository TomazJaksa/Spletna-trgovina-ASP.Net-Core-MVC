using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Voleska.Models;
using Voleska.Services;

namespace Voleska.Controllers
{   
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mailService;
        private readonly Data.ApplicationDbContext _context;
        

        public HomeController(ILogger<HomeController> logger, IMailService mailService, Data.ApplicationDbContext context)
        {
            _logger = logger;
            _mailService = mailService;
            _context = context;
            
        }

      

        public IActionResult Index(string? izbira)
        {
            if (izbira != null)
            {
                var prikazi = izbira;

                return RedirectToAction("Index", "Home", prikazi);
            }
            else {
                ViewData["ReCaptchaResponse"] = TempData["RecaptchaResponse"];
                ViewData["ReCaptchaInfo"] = TempData["ReCaptchaInfo"];
                ViewData["Title"] = "Naslovna stran";
                return View();
            }
            
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<IActionResult> Kontakt(string name, string surname, string email, string subject, string message)
        {
            Pogovor pogovor = new Pogovor();
            pogovor.Izbrisan = false;
            _context.Add(pogovor);
            await _context.SaveChangesAsync();

            Dopisovanje dopisovanje = new Dopisovanje();

            Sporocilo sporocilo = new Sporocilo();

            if (ModelState.IsValid)
            {
                sporocilo.Ime = name;
                sporocilo.Priimek = surname;
                sporocilo.Email = email;
                sporocilo.Tema = subject;
                sporocilo.Vsebina = message;
                sporocilo.Odprto = false;
                sporocilo.Odgovorjeno = false;
                

                _context.Add(sporocilo);
                await _context.SaveChangesAsync();

                dopisovanje.PogovorID = pogovor.ID;
                dopisovanje.SporociloID = sporocilo.ID;
                dopisovanje.Dodan = DateTime.Now;

                _context.Add(dopisovanje);
                await _context.SaveChangesAsync();

                //Posreduj sporočilo lastniku aplikacije
                var sendGrid = "voleska.info@gmail.com";
                var admin = "jaksa.tomaz@gmail.com";
                var adminName = "T.J.";
                var sendGridName = "Voleska info";
                
                
                await _mailService.SendEmailAsync(sendGridName, sendGrid, admin,adminName, name, surname, subject, message, pogovor.ID);


                return RedirectToAction(nameof(Index));
            }


           
           
            
            return RedirectToAction("Index");
        }

    }
}
