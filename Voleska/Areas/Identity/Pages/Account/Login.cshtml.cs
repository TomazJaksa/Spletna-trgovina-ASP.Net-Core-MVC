using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Voleska.Areas.Identity.Data;
using Voleska.Services;

namespace Voleska.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly GooglereCaptchaService _GooglereCaptchaService;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            GooglereCaptchaService googlereCaptchaService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _GooglereCaptchaService = googlereCaptchaService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Vnesite uporabniško ime!")]

            public string Username { get; set; }

            [Required(ErrorMessage = "Vnesite geslo!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Required]
            public string Token { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            //Google reCaptcha
            var _GooglereCaptcha = await _GooglereCaptchaService.ReCaptchaVarification(Input.Token);

            if (_GooglereCaptcha.success && _GooglereCaptcha.score <= 0.5)
            {
                ModelState.AddModelError(string.Empty, "Niste človek!");
                ViewData["ReCaptchaResponse"] = false;
                ViewData["ReCaptchaInfo"] = "Niste prestali ReCaptcha testa!";
                return Page();
            }
            else {
                ViewData["RecaptchaResponse"] = true;
                ViewData["ReCaptchaInfo"] = "ReCaptcha je ocenil, da niste robot!";
            }

            var user = await _userManager.FindByNameAsync(Input.Username);
            if (user!=null) {
                if (user.Aktiven == false)
                {

                    _logger.LogWarning("Uporabniški račun je deaktiviran.");
                    return RedirectToPage("./Lockout");
                }
            }
           

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                

                if (result.Succeeded)
                {
                    TempData["RecaptchaResponse"] = true;
                    TempData["ReCaptchaInfo"] = "ReCaptcha je ocenil, da niste robot!";

                    _logger.LogInformation("User logged in.");
                    
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    TempData["Invalid"] = "Neuspešen vpis!";
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return Page();
                }
            }

            
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
