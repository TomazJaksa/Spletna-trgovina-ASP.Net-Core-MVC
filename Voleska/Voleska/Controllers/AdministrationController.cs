using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;
using Voleska.ViewModel;

namespace Voleska.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly Data.ApplicationDbContext _context;


        public AdministrationController(Data.ApplicationDbContext context, RoleManager<ApplicationRole> roleManager) {
            this.roleManager = roleManager;
            _context = context;
        }

        // GET: Vloge
        public IActionResult Index()
        {
            var roles = roleManager.Roles.AsNoTracking().ToList();

            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid) {
                ApplicationRole identityRole = new ApplicationRole
                {
                    Name = model.RoleName.ToUpper()
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded) {
                    return RedirectToAction("Index","Administration");
                }

                foreach (IdentityError error in result.Errors) {
                    ModelState.AddModelError("", error.Description);  
                }
            
            }

            
            return View(model);
        }

        // GET: Vloge/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vloga = await _context.Roles.FindAsync(id);

            if (vloga == null)
            {
                return NotFound();
            }

          
            return View(vloga);
        }

        // POST: Vloge/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, string Name)
        {
            var vloga = await _context.Roles.FindAsync(Id);

            if (vloga == null) {
                return NotFound();
            }

                if (ModelState.IsValid){
                
                try
                {
                    vloga.Name = Name.ToUpper();
                    vloga.NormalizedName = vloga.Name;

                    _context.Update(vloga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VlogaExists(vloga.Id))
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
            return View(vloga);
        }

        // GET: Vloge/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vloga = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vloga == null)
            {
                return NotFound();
            }

            return View(vloga);
        }

        // POST: Vloge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vloga = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(vloga);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VlogaExists(string id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string roleId, string userId)
        {
            var obstajam = _context.UserRoles.Any(e => e.UserId == userId);

            if (obstajam == false) {
                ApplicationUserRole identityRole = new ApplicationUserRole
                {
                    ApplicationUserID = userId,
                    UserId = userId,
                    ApplicationRoleID = roleId,
                    RoleId = roleId
                };

                _context.UserRoles.Add(identityRole);
                await _context.SaveChangesAsync();

                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovRoleVC), new { roleId = roleId, userId = userId });

            }

            var ustrezen = _context.UserRoles.First(c => c.UserId == userId);

            if (ustrezen == null)
            {
                return NotFound();

            }

            if (ModelState.IsValid)
            {
                


                ApplicationUserRole identityRole = new ApplicationUserRole
                {
                    ApplicationUserID = userId,
                    UserId = userId,
                    ApplicationRoleID = roleId,
                    RoleId = roleId
                };

                _context.UserRoles.Add(identityRole);
                await _context.SaveChangesAsync();

                _context.UserRoles.Remove(ustrezen);
                await _context.SaveChangesAsync();

                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovRoleVC), new { roleId = roleId, userId = userId  });

                /*return RedirectToAction(nameof(Index));*/
            }
            return View(ustrezen);
        }

        public IActionResult RoleReload(string roleId, string userId)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.VlogeVC), new { selectedRole = roleId, userId = userId });
        }

    }
}
