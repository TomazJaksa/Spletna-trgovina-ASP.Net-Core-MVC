using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Voleska.Areas.Identity.Data;

namespace Voleska.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Voleska.Data.ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            Voleska.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone(ErrorMessage = "Dovoljen je samo vnos številk")]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Ime je obvezno!")]
            [DataType(DataType.Text)]
            [Display(Name = "Ime")]
            public string Ime { get; set; }


            [Required(ErrorMessage = "Priimek je obvezen!")]
            [DataType(DataType.Text)]
            [Display(Name = "Priimek")]
            public string Priimek { get; set; }

            public string Ulica { get; set; }


            public string HisnaStevilka { get; set; }


            public string PostnaStevilka { get; set; }


            public string Kraj { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var ime = user.Ime;
            var priimek = user.Priimek;

            var naslov = await _context.Naslovi.FindAsync(user.NaslovID);
            var ulica = naslov.Ulica;
            var hisnaStevilka = naslov.HisnaStevilka;

            var posta = await _context.Poste.FindAsync(naslov.PostaID);
            var postnaStevilka = posta.PostnaStevilka;
            var kraj = posta.Kraj;

            Username = userName;
            Email = email;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Ime = ime,
                Priimek = priimek,
                Ulica = ulica,
                HisnaStevilka = hisnaStevilka,
                PostnaStevilka = postnaStevilka,
                Kraj = kraj
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Problem z nalaganjem uporabnika z  ID : '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Nepričakovana napaka ob nastavljanju telefonske številke";
                    return RedirectToPage();
                }
            }

            var ime = user.Ime;
            if (Input.Ime != ime) {
                user.Ime = Input.Ime;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            var priimek = user.Priimek;
            if (Input.Priimek != ime)
            {
                user.Priimek = Input.Priimek;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            var naslov = await _context.Naslovi.FindAsync(user.NaslovID);
            if (Input.Ulica != naslov.Ulica) {
                naslov.Ulica = Input.Ulica;
                _context.Update(naslov);
                await _context.SaveChangesAsync();
            }

            if (Input.HisnaStevilka != naslov.HisnaStevilka) {
                naslov.HisnaStevilka = Input.HisnaStevilka;
                _context.Update(naslov);
                await _context.SaveChangesAsync();
            }

            var posta = await _context.Poste.FindAsync(naslov.PostaID);
            if (Input.PostnaStevilka != posta.PostnaStevilka)
            {
                posta.PostnaStevilka = Input.PostnaStevilka;
                _context.Update(posta);
                await _context.SaveChangesAsync();
            }

            if (Input.Kraj != posta.Kraj)
            {
                posta.Kraj = Input.Kraj;
                _context.Update(posta);
                await _context.SaveChangesAsync();
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Vaš profil je posodobljen";
            return RedirectToPage();
        }
    }
}
