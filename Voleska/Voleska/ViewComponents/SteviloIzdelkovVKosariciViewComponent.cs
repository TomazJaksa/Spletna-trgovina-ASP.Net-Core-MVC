using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.ViewComponents
{
    public class SteviloIzdelkovVKosariciViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SteviloIzdelkovVKosariciViewComponent(Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _userManager.GetUserId(Request.HttpContext.User);

            var transakcije = await _context.Transakcije
                .FromSqlRaw(@"SELECT * FROM Transakcija WHERE  Zakljuceno = {0} AND ApplicationUserID= {1}", false, user)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var izdelkiVKosarici = await _context.Narocila
                .FromSqlRaw(@"SELECT * FROM Narocilo WHERE  TransakcijaID = {0}", -1)
                .AsNoTracking()
                .ToListAsync();

            if (transakcije != null)
            {
                 izdelkiVKosarici = await _context.Narocila
                .FromSqlRaw(@"SELECT * FROM Narocilo WHERE  TransakcijaID = {0}", transakcije.ID)
                .AsNoTracking()
                .ToListAsync();

                ViewData["TransakcijaID"] = transakcije.ID;
            }
           

            return View(izdelkiVKosarici);
        }
    }
}
