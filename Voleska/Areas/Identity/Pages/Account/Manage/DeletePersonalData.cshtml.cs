using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Voleska.Areas.Identity.Data;
using System.Linq;
using System.Collections.Generic;
using Voleska.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Voleska.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly Voleska.Data.ApplicationDbContext _context;


        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            Voleska.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Geslo je obvezno!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

       
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Geslo je napačno.");
                    ViewData["Error"] = "Geslo je napačno";
                    return Page();
                }
            }

            

            var naslov = await _context.Naslovi.FindAsync(user.NaslovID);
            var posta = await _context.Poste.FindAsync(naslov.PostaID);


            var lajkKomentarjev = from m in _context.LajkaniKomentarji
                                  select m;

            lajkKomentarjev = lajkKomentarjev.Where(s => s.ApplicationUserID == user.Id);

            foreach (var lajk in lajkKomentarjev)
            {
                _context.LajkaniKomentarji.Remove(lajk);
                var komentar = _context.Komentarji.FirstOrDefault(u => u.ID == lajk.KomentarID);
                if (lajk.Lajk == true)
                {
                    komentar.SteviloLike -= 1;
                    _context.Komentarji.Update(komentar);
                }
                else {
                    komentar.SteviloDislike -= 1;
                    _context.Komentarji.Update(komentar);
                }
                
            }
            await _context.SaveChangesAsync();


            var komentarji = from m in _context.Komentarji
                             select m;

            komentarji = komentarji.Where(s => s.ApplicationUserID == user.Id);

            foreach (var komentar in komentarji)
            {
                //_context.Komentarji.Remove(komentar);
                IzbrisiKomentar(komentar.ID);
            }
            await _context.SaveChangesAsync();





            var lajkBlogov = from m in _context.LajkaniBlogi
                             select m;

            lajkBlogov = lajkBlogov.Where(s => s.ApplicationUserID == user.Id);

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

            oceneIzdelkov = oceneIzdelkov.Where(s => s.ApplicationUserID == user.Id);

            foreach (var ocena in oceneIzdelkov)
            {
                _context.OceneIzdelkov.Remove(ocena);
            }
            await _context.SaveChangesAsync();


            var transakcije = from m in _context.Transakcije
                              select m;

            transakcije = transakcije.Where(s => s.ApplicationUserID == user.Id);

            foreach (var transakcija in transakcije)
            {
                _context.Transakcije.Remove(transakcija);
            }
            await _context.SaveChangesAsync();


            var novice = from m in _context.Novice
                         select m;

            novice = novice.Where(s => s.Email == user.Email);

            foreach (var narocilo in novice)
            {
                _context.Novice.Remove(narocilo);
            }
            await _context.SaveChangesAsync();


            _context.Naslovi.Remove(naslov);
            await _context.SaveChangesAsync();

            _context.Poste.Remove(posta);
            await _context.SaveChangesAsync();

            

           // var result = await _userManager.DeleteAsync(user);
            //user.Aktiven = false;

            //await _userManager.UpdateAsync(user);

            var userId = await _userManager.GetUserIdAsync(user);
            // if (!result.Succeeded)
            // {
            //     throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            // }

           
            await _signInManager.SignOutAsync();
           
            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
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
            var komentar = _context.Komentarji.Find(id);

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
    }
}
