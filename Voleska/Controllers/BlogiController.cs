using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Voleska.Data;
using Voleska.Models;
using Voleska.ViewModel;

using Microsoft.AspNetCore.Http;
using LazZiya.ImageResize;
using Tweetinvi.Controllers.Upload;

namespace Voleska.Controllers
{
    [AllowAnonymous]
    public class BlogiController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;


        public BlogiController(Data.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: Blogs
        public async Task<IActionResult> Index(string searchString, int? stevilkaStrani, int? kliknjenLinkStrani, string filtriranje)
        {
            ViewData["UserID"] = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["BlogerID"] = "b3705ab3-9b1e-4674-b921-57e1476e9522";
            ViewData["DrugiKrog"] = false;
            var user  = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var obstojLajka = _context.LajkaniBlogi
                   .FromSqlRaw(@"SELECT * FROM LajkanjeBlogov WHERE ApplicationUserID= {0} ", user)
                   .Count();

            ViewData["ObstojLajka"] = obstojLajka;

            var blogi = from m in _context.Blogi
                          select m;

            if (searchString != null || filtriranje != null)
            {
                stevilkaStrani = 1;

                if (kliknjenLinkStrani != null)
                {
                    stevilkaStrani = kliknjenLinkStrani;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    blogi = blogi.Where(s => s.Naslov.Contains(searchString));
                }

                switch (filtriranje)
                {
                    case "datumPadajoce":
                        blogi = blogi.OrderByDescending(s => s.Dodan);
                        ViewData["IzbranFilter"] = "Najnovejši";
                        break;
                    case "datumNarascajoce":
                        blogi = blogi.OrderBy(s => s.Dodan);
                        ViewData["IzbranFilter"] = "Najstarejši";
                        break;
                    case "priljubljenostPadajoce":
                        blogi = blogi.OrderByDescending(s => s.SteviloLike);
                        ViewData["IzbranFilter"] = "Priljubljenost";
                        break;
                }
            }
              

            
            ViewData["SearchString"] = searchString;
            ViewData["Filtriranje"] = filtriranje;
            if (stevilkaStrani == null)
            {
                ViewData["StevilkaStrani"] = 1;
            }
            else
            {
                ViewData["StevilkaStrani"] = stevilkaStrani;
            }

            List<Blog> ustrezniBlogi = await blogi.Include(i => i.LajkaniBlogi).ToListAsync();
            int steviloPrikazanihBlogov = 3;

            return View(PaginatedList<Blog>.CreateAsync(ustrezniBlogi, stevilkaStrani ?? 1, steviloPrikazanihBlogov));
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogi
                .Include(i => i.LajkaniBlogi)
                .Include(i => i.Komentarji)
                    .ThenInclude(i => i.ApplicationUser)
                .Include(i => i.Komentarji)
                    .ThenInclude(i => i.LajkaniKomentarji)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (blog == null)
            {
                return NotFound();
            }

            ViewData["UserID"] = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["BlogerID"] = "b3705ab3-9b1e-4674-b921-57e1476e9522";
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

           

            return View(blog);
        }
        [Authorize]
        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Naslov,Slika,Povzetek,Clanek,Bloger,Dodan,Posodobljen,SteviloLike,SteviloDislike")] BlogViewModel vm)
        {
            Blog blog = new Blog();
            if (ModelState.IsValid)
            {
                blog.Naslov = vm.Naslov;
                string imeSlike = UploadFile(vm);
                blog.Slika = imeSlike; 
                blog.Povzetek = vm.Povzetek;
                blog.Clanek = vm.Clanek;
                blog.Bloger = vm.Bloger;
                blog.Dodan = DateTime.Now;
                blog.Posodobljen = DateTime.Now;
                blog.SteviloLike = 0;
                blog.SteviloDislike = 0;
                
                _context.Add(blog);
                await _context.SaveChangesAsync();
                
                LajkanjeBlogov lajkanjeBloga = new LajkanjeBlogov();

                lajkanjeBloga.BlogID = blog.ID;
                lajkanjeBloga.Dodan = DateTime.Now;
                lajkanjeBloga.Posodobljen = DateTime.Now;
                lajkanjeBloga.Lajk = null;
                lajkanjeBloga.ApplicationUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(lajkanjeBloga);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
                
            }
            return RedirectToAction(nameof(Index));
        }

        private string UploadFile(BlogViewModel vm)
        {
            string imeSlike = null;
            if (vm.Slika != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "C:/Users/CRIMSON/source/repos/Voleska/Voleska/wwwroot/slike");
                imeSlike = Guid.NewGuid().ToString() + "-" + vm.Slika.FileName;
                string filePath = Path.Combine(uploadDir, imeSlike);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vm.Slika.CopyTo(fileStream);
                }
            }

            var photo_thumbnail = "C:/Users/CRIMSON/source/repos/Voleska/Voleska/wwwroot/thumbnail/" + imeSlike;
            ResizeImage(vm.Slika, photo_thumbnail, 100);
            /*
            var photo_jumbotron = "C:/Users/CRIMSON/source/repos/Voleska/Voleska/wwwroot/jumbotron/" + imeSlike;
            ResizeImage(vm.Slika, photo_jumbotron, 1500);
            */
            return imeSlike;
        }

        public void ResizeImage(IFormFile uploadedFile, string desiredThumbPath, int desiredWidth = 0, int desiredHeight = 0)
        {
            string webroot = WebHostEnvironment.WebRootPath;

            if (uploadedFile.Length > 0)
            {
                using (var stream = uploadedFile.OpenReadStream())
                {
                    var uploadedImage = System.Drawing.Image.FromStream(stream);

                    //decide how to scale dimensions
                    if (desiredHeight == 0 && desiredWidth > 0)
                    {
                        var img = ImageResize.ScaleByWidth(uploadedImage, desiredWidth); // returns System.Drawing.Image file
                        img.SaveAs(Path.Combine(webroot, desiredThumbPath));
                    }
                    else if (desiredWidth == 0 && desiredHeight > 0)
                    {
                        var img = ImageResize.ScaleByHeight(uploadedImage, desiredHeight); // returns System.Drawing.Image file
                        img.SaveAs(Path.Combine(webroot, desiredThumbPath));
                    }
                    else
                    {
                        var img = ImageResize.Scale(uploadedImage, desiredWidth, desiredHeight); // returns System.Drawing.Image file
                        img.SaveAs(Path.Combine(webroot, desiredThumbPath));
                    }
                }
            }
            return;
        }

        // POST: LajkaniBlogi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDetailsLajk(int blogId, bool like)
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
                }
                else
                {

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

                        if (noviLike > 0)
                        {
                            noviLike = trenutniDislike.SteviloLike - 1;
                        }


                        var increase = _context.Database
                       .ExecuteSqlRaw(
                           @"UPDATE Blog
                            SET SteviloDislike = {0}, SteviloLike = {1}, Posodobljen = {2}                      
                            WHERE ID = {3}", noviDislike, noviLike, trenutniDislike.Posodobljen, blogId);

                    }
                    else
                    {
                        obstojLajka.Lajk = true;
                        var trenutniLike = await _context.Blogi
                        .FromSqlRaw(@"SELECT * FROM Blog WHERE  ID = {0}", blogId)
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




                return RedirectToAction("Details", "Blogi", new { @id = blogId });
            }
            ViewData["BlogID"] = new SelectList(_context.Blogi, "ID", "ID", lajkanjeBlogov1.BlogID);
            ViewData["UporabnikID"] = new SelectList(_context.Users, "Id", "Id", lajkanjeBlogov1.ApplicationUserID);
            return RedirectToAction("Details", "Blogi", new { @id = blogId });
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogi.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            BlogEditViewModel blogEditViewModel = new BlogEditViewModel
            {
                ID = blog.ID,
                Naslov = blog.Naslov,
                ObstojecaSlika = blog.Slika,
                Povzetek = blog.Povzetek,
                Clanek = blog.Clanek,
                Bloger = blog.Bloger,
                Dodan = blog.Dodan,
                Posodobljen = blog.Posodobljen,
                SteviloLike = blog.SteviloLike,
                SteviloDislike = blog.SteviloDislike
            };
            return View(blogEditViewModel);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Naslov,Slika,Povzetek,Clanek,Bloger")] BlogViewModel vm)
        {
            var blog = await _context.Blogi.FindAsync(id);

            if (id != blog.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blog.Naslov = vm.Naslov;
                    string imeSlike = UploadFile(vm);
                    if (imeSlike != null)
                    {
                        blog.Slika = imeSlike;
                    }
                    
                    blog.Povzetek = vm.Povzetek;
                    blog.Clanek = vm.Clanek;
                    blog.Bloger = vm.Bloger;
                    blog.Posodobljen = DateTime.Now;
                    
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.ID))
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
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogi
                .FirstOrDefaultAsync(m => m.ID == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogi.FindAsync(id);
            _context.Blogi.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogi.Any(e => e.ID == id);
        }
    }
}
