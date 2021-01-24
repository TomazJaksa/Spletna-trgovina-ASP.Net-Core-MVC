using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Voleska.Data;
using Voleska.Models;

namespace Voleska.Controllers
{
    public class LajkaniKomentarjiController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public LajkaniKomentarjiController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LajkaniKomentarji
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.LajkaniKomentarji.Include(l => l.Komentar).Include(l => l.ApplicationUser);
            return View(await storeContext.ToListAsync());
        }

        // GET: LajkaniKomentarji/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lajkanjeKomentarjev = await _context.LajkaniKomentarji
                .Include(l => l.Komentar)
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lajkanjeKomentarjev == null)
            {
                return NotFound();
            }

            return View(lajkanjeKomentarjev);
        }

        // GET: LajkaniKomentarji/Create
        public IActionResult Create(int komentarId)
        {
            ViewBag.KomentarId = komentarId;
            return View();
        }

        // POST: LajkaniBlogi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public async Task<IActionResult> Create(int komentarId, bool like, int blogId)
        {

            LajkanjeKomentarjev lajkanjeKomentarjev1 = new LajkanjeKomentarjev();

            if (ModelState.IsValid)
            {

                // najprej preverimo ali je ta komentar, ta uporabnik že lajkal/dislajkal.
                // za to rabimo komentarId, ApplicationUserID

              

                var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);


                var obstojLajka = await _context.LajkaniKomentarji
                    .FromSqlRaw(@"SELECT * FROM LajkanjeKomentarjev WHERE  KomentarID = {0} AND ApplicationUserID= {1}", komentarId, user)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (obstojLajka == null)
                {
                    LajkanjeKomentarjev lajkanjeKomentarjev = new LajkanjeKomentarjev();
                    lajkanjeKomentarjev.Lajk = like;

                    if (lajkanjeKomentarjev.Lajk == false)
                    {
                        var trenutniDislike = await _context.Komentarji
                        .FromSqlRaw(@"SELECT * FROM Komentar WHERE  ID = {0}", komentarId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                        trenutniDislike.Posodobljen = DateTime.Now;

                        int noviDislike = trenutniDislike.SteviloDislike + 1;


                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Komentar
                            SET SteviloDislike = {0}                      
                            WHERE ID = {1}", noviDislike, komentarId);

                    }
                    else
                    {

                        var trenutniLike = await _context.Komentarji
                        .FromSqlRaw(@"SELECT * FROM Komentar WHERE  ID = {0}", komentarId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                        trenutniLike.Posodobljen = DateTime.Now;

                        int noviLike = trenutniLike.SteviloLike + 1;

                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Komentar
                            SET SteviloLike = {0}                      
                            WHERE ID = {1}", noviLike, komentarId);


                    }

                    lajkanjeKomentarjev.Posodobljen = DateTime.Now;
                    lajkanjeKomentarjev.Dodan = DateTime.Now;
                    lajkanjeKomentarjev.ApplicationUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    lajkanjeKomentarjev.KomentarID = komentarId;
                    _context.Add(lajkanjeKomentarjev);
                    await _context.SaveChangesAsync();

                    var ustrezenKomentar = await _context.Komentarji.FindAsync(komentarId);

                    var dobiId = blogId;
                    var dobiKomentarId = komentarId;
                    var dobiLike = "";
                    var dobiDislike = "";
                    var dobiSteviloDislike = ustrezenKomentar.SteviloDislike;
                    var dobiSteviloLike = ustrezenKomentar.SteviloLike;


                    if (lajkanjeKomentarjev.Lajk == true)
                    {
                        dobiLike = "disabled";
                    }
                    else
                    {
                        dobiDislike = "disabled";
                    }


                    return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovLajkKomentarVC), new {komentarId = dobiKomentarId, blogId = dobiId, like = dobiLike, dislike = dobiDislike, steviloDislike = dobiSteviloDislike, steviloLike = dobiSteviloLike });

                }
                else
                {
                    if (obstojLajka.Lajk == null) {
                        obstojLajka.Lajk = !like;
                    }

                    if (obstojLajka.Lajk == true)
                    {
                        obstojLajka.Lajk = false;
                        var trenutniDislike = await _context.Komentarji
                        .FromSqlRaw(@"SELECT * FROM Komentar WHERE  ID = {0}", komentarId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                        trenutniDislike.Posodobljen = DateTime.Now;

                        int noviDislike = trenutniDislike.SteviloDislike + 1;
                        int noviLike = trenutniDislike.SteviloLike;

                        if (noviLike > 0)
                        {
                            noviLike = trenutniDislike.SteviloLike - 1;
                        }


                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Komentar
                            SET SteviloDislike = {0}, SteviloLike = {1}, Posodobljen = {2}                      
                            WHERE ID = {3}", noviDislike, noviLike, trenutniDislike.Posodobljen, komentarId);

                    }
                    else
                    {
                        obstojLajka.Lajk = true;
                        var trenutniLike = await _context.Komentarji
                        .FromSqlRaw(@"SELECT * FROM Komentar WHERE  ID = {0}", komentarId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                        trenutniLike.Posodobljen = DateTime.Now;

                        int noviDislike = trenutniLike.SteviloDislike;

                        if (noviDislike > 0)
                        {
                            noviDislike = trenutniLike.SteviloDislike - 1;
                        }

                        int noviLike = trenutniLike.SteviloLike + 1;

                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Komentar
                            SET SteviloLike = {0}, SteviloDislike = {1}, Posodobljen = {2}                      
                            WHERE ID = {3}", noviLike, noviDislike, trenutniLike.Posodobljen, komentarId);


                    }

                    obstojLajka.Posodobljen = DateTime.Now;

                    var rowsAffected = _context.Database
                        .ExecuteSqlRaw(
                            @"UPDATE LajkanjeKomentarjev
                            SET Lajk = {0}, Posodobljen = {1}                      
                            WHERE KomentarID = {2} AND ApplicationUserID = {3}", obstojLajka.Lajk, obstojLajka.Posodobljen, komentarId, user);

                    ViewData["DrugiKrog"] = true;
                    await _context.SaveChangesAsync();
                }


                var ustrezenKomentar2 = await _context.Komentarji.FindAsync(komentarId);

                var dobiId2 = blogId;
                var dobiKomentarId2 = komentarId;
                var dobiLike2 = "";
                var dobiDislike2 = "";
                var dobiSteviloDislike2 = ustrezenKomentar2.SteviloDislike;
                var dobiSteviloLike2 = ustrezenKomentar2.SteviloLike;


                if (obstojLajka.Lajk == true)
                {
                    dobiLike2 = "disabled";
                }
                else
                {
                    dobiDislike2 = "disabled";
                }


                return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.PrenosPodatkovLajkKomentarVC), new { komentarId = dobiKomentarId2, blogId = dobiId2, like = dobiLike2, dislike = dobiDislike2, steviloDislike = dobiSteviloDislike2, steviloLike = dobiSteviloLike2 });


                /*
                return RedirectToAction("Details", "Blogi", new { @id = blogId });
                */
            }
            ViewData["KomentarID"] = new SelectList(_context.Komentarji, "ID", "ID", lajkanjeKomentarjev1.KomentarID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", lajkanjeKomentarjev1.ApplicationUserID);
            return View(lajkanjeKomentarjev1);
        }

        public IActionResult GumbiReload(int komentarId, int blogId, string lajk, string dislajk)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.LajkKomentarVC), new {komentarId = komentarId,  blogId = blogId, like = lajk, dislike = dislajk });
        }

        public IActionResult SteviloLikeReload(int steviloLike)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.SteviloLikeKomentarVC), new { steviloLike = steviloLike });
        }

        public IActionResult SteviloDislikeReload(int steviloDislike)
        {
            return ViewComponent(nameof(Voleska.ViewComponents.ViewComponents.SteviloDislikeKomentarVC), new { steviloDislike = steviloDislike });
        }


        // GET: LajkaniKomentarji/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lajkanjeKomentarjev = await _context.LajkaniKomentarji.FindAsync(id);
            if (lajkanjeKomentarjev == null)
            {
                return NotFound();
            }
            ViewData["KomentarID"] = new SelectList(_context.Komentarji, "ID", "ID", lajkanjeKomentarjev.KomentarID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", lajkanjeKomentarjev.ApplicationUserID);
            return View(lajkanjeKomentarjev);
        }

        // POST: LajkaniKomentarji/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Lajk,Dodan,Posodobljen,UporabnikID,KomentarID")] LajkanjeKomentarjev lajkanjeKomentarjev)
        {
            if (id != lajkanjeKomentarjev.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lajkanjeKomentarjev);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LajkanjeKomentarjevExists(lajkanjeKomentarjev.ID))
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
            ViewData["KomentarID"] = new SelectList(_context.Komentarji, "ID", "ID", lajkanjeKomentarjev.KomentarID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", lajkanjeKomentarjev.ApplicationUserID);
            return View(lajkanjeKomentarjev);
        }

        // GET: LajkaniKomentarji/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lajkanjeKomentarjev = await _context.LajkaniKomentarji
                .Include(l => l.Komentar)
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lajkanjeKomentarjev == null)
            {
                return NotFound();
            }

            return View(lajkanjeKomentarjev);
        }

        // POST: LajkaniKomentarji/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lajkanjeKomentarjev = await _context.LajkaniKomentarji.FindAsync(id);
            _context.LajkaniKomentarji.Remove(lajkanjeKomentarjev);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LajkanjeKomentarjevExists(int id)
        {
            return _context.LajkaniKomentarji.Any(e => e.ID == id);
        }
    }
}
