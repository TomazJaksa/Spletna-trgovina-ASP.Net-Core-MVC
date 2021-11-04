using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Voleska.Areas.Identity.Data;
using Voleska.Models;
using Voleska.Services;

namespace Voleska.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly Voleska.Data.ApplicationDbContext _context;
        private readonly GooglereCaptchaService _GooglereCaptchaService;


        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            Voleska.Data.ApplicationDbContext context,
            GooglereCaptchaService googlereCaptchaService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _GooglereCaptchaService = googlereCaptchaService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Uporabniško ime je obvezno!")]
            [DataType(DataType.Text)]
            [Display(Name = "Uporabniško ime")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Ime je obvezno!")]
            [DataType(DataType.Text)]
            [Display(Name = "Ime")]
            public string Ime { get; set; }

            [Required(ErrorMessage = "Priimek je obvezen!")]
            [DataType(DataType.Text)]
            [Display(Name = "Priimek")]
            public string Priimek { get; set; }

            [Required(ErrorMessage = "E-naslov je obvezen!")]
            [EmailAddress(ErrorMessage = "Uporabite format: lep.primer@hotmail.com")]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required(ErrorMessage = "Geslo je obvezno!")]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "Geslo mora vsebovati vsaj 10 znakov", MinimumLength = 10)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,100}$",
         ErrorMessage = "Geslo mora vsebovati vsaj 10 znakov, eno veliko črko, eno malo črko,eno številko in en poseben znak ( ?, ! , * , + , - ).")]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Potrebno je potrditi geslo!")]
            [DataType(DataType.Password)]
            [Display(Name = "Potrditev gesla")]
            [Compare("Password", ErrorMessage = "Gesli se ne ujemata.")]
            public string ConfirmPassword { get; set; }

            //dodamo polja za Naslov in pošto !
            
            public string Ulica { get; set; }

            
            public string HisnaStevilka { get; set; }

            
            public string PostnaStevilka { get; set; }

            
            public string Kraj { get; set; }

            public bool Novice { get; set; }

            [Required]
            public string Token { get; set; }

        }
        
        

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            bool narocnina = Input.Novice;

            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //Google reCaptcha
            var _GooglereCaptcha = await _GooglereCaptchaService.ReCaptchaVarification(Input.Token);

            if (_GooglereCaptcha.success && _GooglereCaptcha.score <= 0.5)
            {
                ModelState.AddModelError(string.Empty, "Niste človek!");
                ViewData["ReCaptchaResponse"] = false;
                ViewData["ReCaptchaInfo"] = "Niste prestali ReCaptcha testa!";
                return Page();
            }
            else
            {
                ViewData["RecaptchaResponse"] = true;
                ViewData["ReCaptchaInfo"] = "ReCaptcha je ocenil, da niste robot!";
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Username, Ime = Input.Ime, Priimek = Input.Priimek, Email = Input.Email };

                
                if (narocnina==true) {
                    var naroci = new Novice { Email = user.Email, UporabniskoIme = user.UserName, Dodan = DateTime.Now, Posodobljen = DateTime.Now};
                    _context.Add(naroci);
                    await _context.SaveChangesAsync();

                }
                // Dodamo Nove objekte naslov in pošta, ter shranimo vrednosti notri

                var naslov = new Naslov { Ulica = Input.Ulica, HisnaStevilka = Input.HisnaStevilka };
                
                naslov.ApplicationUserID = user.Id;
                naslov.Dodan = DateTime.Now;
                naslov.Posodobljen = DateTime.Now;

                var posta = new Posta { PostnaStevilka = Input.PostnaStevilka, Kraj = Input.Kraj };
                posta.Dodan = DateTime.Now;
                posta.Posodobljen = DateTime.Now;
                _context.Add(posta);
                await _context.SaveChangesAsync();

                naslov.PostaID = posta.ID;
                _context.Add(naslov);
                await _context.SaveChangesAsync();

                user.NaslovID = naslov.ID;
                user.Dodan = DateTime.Now;
                user.Posodobljen = DateTime.Now;
                user.Aktiven = true;
                
                var vloga = from m in _context.Roles
                                  select m;

                

                var result = await _userManager.CreateAsync((ApplicationUser)user, Input.Password);
                

                vloga = vloga.Where(s => s.Name == "UPORABNIK");

                string imeVloge = "";
                string idVloge = "";
                foreach (var v in vloga)
                {
                    imeVloge = v.NormalizedName;
                    idVloge = v.Id;
                }
                ApplicationUserRole newUserRole = new ApplicationUserRole
                {
                    ApplicationRoleID = idVloge,
                    ApplicationUserID = user.Id,
                    RoleId = idVloge,
                    UserId = user.Id

                };

                /*await _userManager.AddToRoleAsync((ApplicationUser)user, imeVloge);*/
                _context.Add(newUserRole);
                await _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync((ApplicationUser)user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {

                        TempData["RecaptchaResponse"] = true;
                        TempData["ReCaptchaInfo"] = "ReCaptcha je ocenil, da niste robot!";


                        await _signInManager.SignInAsync((ApplicationUser)user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
