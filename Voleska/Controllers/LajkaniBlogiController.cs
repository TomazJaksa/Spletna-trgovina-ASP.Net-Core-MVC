using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NHibernate.Mapping;
using Voleska.Data;
using Voleska.Models;

namespace Voleska.Controllers
{
    public class LajkaniBlogiController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public LajkaniBlogiController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LajkaniBlogi
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.LajkaniBlogi.Include(l => l.Blog).Include(l => l.ApplicationUser);
            return View(await storeContext.ToListAsync());
        }

        // GET: LajkaniBlogi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lajkanjeBlogov = await _context.LajkaniBlogi
                .Include(l => l.Blog)
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lajkanjeBlogov == null)
            {
                return NotFound();
            }

            return View(lajkanjeBlogov);
        }

        // GET: LajkaniBlogi/Create
        public IActionResult Create()
        {
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "ID");
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: LajkaniBlogi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(int blogId, bool like, int? stevilkaStrani, string searchString, string filtriranje )
        {

            LajkanjeBlogov lajkanjeBlogov1 = new LajkanjeBlogov();

            if (ModelState.IsValid)
            {

                // najprej preverimo ali je ta blog, ta uporabnik že lajkal/dislajkal.
                // za to rabimo blogId, ApplicationUserID

                /* var obstojLajka = _context.Blogi.FirstOrDefault(m => m.ID == blogId );*/

                

                var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                
                var obstojLajka = await _context.LajkaniBlogi
                    .FromSqlRaw(@"SELECT * FROM LajkanjeBlogov WHERE  BlogID = {0} AND ApplicationUserID= {1}", blogId, user)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (obstojLajka == null)
                {
                    LajkanjeBlogov lajkanjeBlogov = new LajkanjeBlogov();
                    lajkanjeBlogov.Lajk = like;

                    if (lajkanjeBlogov.Lajk == false)
                    {
                        var trenutniDislike = await _context.Blogi
                        .FromSqlRaw(@"SELECT * FROM Blog WHERE  ID = {0}", blogId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                        trenutniDislike.Posodobljen = DateTime.Now;

                        int noviDislike = trenutniDislike.SteviloDislike + 1;
                        

                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Blog
                            SET SteviloDislike = {0}                      
                            WHERE ID = {1}", noviDislike, blogId);

                    }
                    else
                    {
                        
                        var trenutniLike = await _context.Blogi
                        .FromSqlRaw(@"SELECT * FROM Blog WHERE  ID = {0}", blogId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                        trenutniLike.Posodobljen = DateTime.Now;

                        int noviLike = trenutniLike.SteviloLike + 1;

                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Blog
                            SET SteviloLike = {0}                      
                            WHERE ID = {1}", noviLike, blogId);


                    }

                    lajkanjeBlogov.Posodobljen = DateTime.Now;
                    lajkanjeBlogov.Dodan = DateTime.Now;
                    lajkanjeBlogov.ApplicationUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    lajkanjeBlogov.BlogID = blogId;
                    _context.Add(lajkanjeBlogov);
                    await _context.SaveChangesAsync();


                    var ustrezenBlog = await _context.Blogi.FindAsync(blogId);

                    var dobiId = blogId;
                    var dobiLike = "";
                    var dobiDislike = "";
                    var dobiSteviloDislike = ustrezenBlog.SteviloDislike;
                    var dobiSteviloLike = ustrezenBlog.SteviloLike;

                    
                        if (lajkanjeBlogov.Lajk == true)
                        {
                            dobiLike = "disabled";
                        }
                        else {
                            dobiDislike = "disabled";
                        }


                    return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovLajkVC), new { blogId = dobiId, like=dobiLike, dislike=dobiDislike, steviloDislike=dobiSteviloDislike, steviloLike = dobiSteviloLike });

                }
                else {

                    if (obstojLajka.Lajk == true)
                    {
                        obstojLajka.Lajk = false;
                        var trenutniDislike = await _context.Blogi
                        .FromSqlRaw(@"SELECT * FROM Blog WHERE  ID = {0}", blogId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                        trenutniDislike.Posodobljen = DateTime.Now;

                        int noviDislike = trenutniDislike.SteviloDislike + 1;
                        int noviLike = trenutniDislike.SteviloLike;

                        if (noviLike > 0) {
                            noviLike = trenutniDislike.SteviloLike - 1;
                        }
                        

                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Blog
                            SET SteviloDislike = {0}, SteviloLike = {1}, Posodobljen = {2}                      
                            WHERE ID = {3}", noviDislike, noviLike, trenutniDislike.Posodobljen, blogId);

                    }
                    else {
                        obstojLajka.Lajk = true;
                        var trenutniLike = await _context.Blogi
                        .FromSqlRaw(@"SELECT * FROM Blog WHERE  ID = {0}", blogId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                        trenutniLike.Posodobljen = DateTime.Now;

                        int noviDislike = trenutniLike.SteviloDislike;

                        if (noviDislike > 0) {
                            noviDislike = trenutniLike.SteviloDislike - 1;
                        }
                        
                        int noviLike = trenutniLike.SteviloLike + 1;

                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Blog
                            SET SteviloLike = {0}, SteviloDislike = {1}, Posodobljen = {2}                      
                            WHERE ID = {3}", noviLike, noviDislike, trenutniLike.Posodobljen, blogId);


                    }

                    obstojLajka.Posodobljen = DateTime.Now;

                    var rowsAffected = _context.Database
                        .ExecuteSqlRaw(
                            @"UPDATE LajkanjeBlogov
                            SET Lajk = {0}, Posodobljen = {1}                      
                            WHERE BlogID = {2} AND ApplicationUserID = {3}", obstojLajka.Lajk, obstojLajka.Posodobljen, blogId, user);

                    ViewData["DrugiKrog"] = true;
                    await _context.SaveChangesAsync();
                }

                var ustrezenBlog2 = await _context.Blogi.FindAsync(blogId);

                var dobiId2 = blogId;
                var dobiLike2 = "";
                var dobiDislike2 = "";
                var dobiSteviloDislike2 = ustrezenBlog2.SteviloDislike;
                var dobiSteviloLike2 = ustrezenBlog2.SteviloLike;


                if (obstojLajka.Lajk == true)
                {
                    dobiLike2 = "disabled";
                }
                else
                {
                    dobiDislike2 = "disabled";
                }


                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovLajkVC), new { blogId = dobiId2, like = dobiLike2, dislike = dobiDislike2, steviloDislike = dobiSteviloDislike2, steviloLike = dobiSteviloLike2 });


                /*
                return RedirectToAction("Index", "Blogi", new
                {
                    stevilkaStrani = stevilkaStrani,
                    searchString = searchString,
                    filtriranje = filtriranje

                }, $"blog-{blogId}");
                */
            }
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "ID", lajkanjeBlogov1.BlogID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", lajkanjeBlogov1.ApplicationUserID);
            return View(lajkanjeBlogov1);
        }

        public IActionResult GumbiReload(int blogId, string lajk, string dislajk)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.LajkVC), new { blogId = blogId, like = lajk, dislike = dislajk });
        }

        public IActionResult SteviloLikeReload(int steviloLike)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.SteviloLikeVC), new { steviloLike = steviloLike });
        }

        public IActionResult SteviloDislikeReload(int steviloDislike)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.SteviloDislikeVC), new { steviloDislike = steviloDislike });
        }


        // GET: LajkaniBlogi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lajkanjeBlogov = await _context.LajkaniBlogi.FindAsync(id);
            if (lajkanjeBlogov == null)
            {
                return NotFound();
            }
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "ID", lajkanjeBlogov.BlogID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", lajkanjeBlogov.ApplicationUserID);
            return View(lajkanjeBlogov);
        }

        // POST: LajkaniBlogi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Lajk,Dodan,Posodobljen,UporabnikID,BlogID")] LajkanjeBlogov lajkanjeBlogov)
        {
            if (id != lajkanjeBlogov.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lajkanjeBlogov);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LajkanjeBlogovExists(lajkanjeBlogov.ID))
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
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "ID", lajkanjeBlogov.BlogID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", lajkanjeBlogov.ApplicationUserID);
            return View(lajkanjeBlogov);
        }

        // GET: LajkaniBlogi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lajkanjeBlogov = await _context.LajkaniBlogi
                .Include(l => l.Blog)
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lajkanjeBlogov == null)
            {
                return NotFound();
            }

            return View(lajkanjeBlogov);
        }

        // POST: LajkaniBlogi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lajkanjeBlogov = await _context.LajkaniBlogi.FindAsync(id);
            _context.LajkaniBlogi.Remove(lajkanjeBlogov);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LajkanjeBlogovExists(int id)
        {
            return _context.LajkaniBlogi.Any(e => e.ID == id);
        }
    }
}
