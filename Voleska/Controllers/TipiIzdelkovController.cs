using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Voleska.Data;
using Voleska.Models;
using Voleska.ViewModel;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

using Microsoft.AspNetCore.Http;
using LazZiya.ImageResize;
using Tweetinvi.Controllers.Upload;

namespace Voleska.Controllers
{
    public class TipiIzdelkovController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public TipiIzdelkovController(Data.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: TipiIzdelkov
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipiIzdelkov.ToListAsync());
        }

        // GET: TipiIzdelkov/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipIzdelka = await _context.TipiIzdelkov
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipIzdelka == null)
            {
                return NotFound();
            }

            return View(tipIzdelka);
        }

        // GET: TipiIzdelkov/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipiIzdelkov/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ime,Slika")] TipIzdelkaViewModel vm)
        {
            TipIzdelka tipIzdelka = new TipIzdelka();

            if (ModelState.IsValid)
            {
                tipIzdelka.Ime = vm.Ime;
                string imeSlike = UploadFile(vm);
                tipIzdelka.Slika = imeSlike;
                _context.Add(tipIzdelka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipIzdelka);
        }

        private string UploadFile(TipIzdelkaViewModel vm)
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

            var katalogSlika = "C:/Users/CRIMSON/source/repos/Voleska/Voleska/wwwroot/katalogSlike/" + imeSlike;
            ResizeImage(vm.Slika, katalogSlika, 290);

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


        // GET: TipiIzdelkov/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            TipIzdelka tipIzdelka = await _context.TipiIzdelkov
                    .FromSqlRaw(@"SELECT * FROM TipIzdelka WHERE  ID = {0}", id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            TipIzdelkaEditViewModel tipIzdelkaEditViewModel = new TipIzdelkaEditViewModel {
                ID = tipIzdelka.ID,
                Ime = tipIzdelka.Ime,
                
                ObstojecaSlika = tipIzdelka.Slika
                
            
            };
            return View(tipIzdelkaEditViewModel);
        }

        // POST: TipiIzdelkov/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ime, Slika")] TipIzdelkaViewModel vm)
        {
            var tipIzdelka = await _context.TipiIzdelkov
                    .FromSqlRaw(@"SELECT * FROM TipIzdelka WHERE  ID = {0}", id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            if (id != tipIzdelka.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tipIzdelka.Ime = vm.Ime;
                    string imeSlike = UploadFile(vm);
                    if (imeSlike != null)
                    {
                        tipIzdelka.Slika = imeSlike;
                    }
                    

                    _context.Update(tipIzdelka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipIzdelkaExists(tipIzdelka.ID))
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
            return View(tipIzdelka);
        }

        // GET: TipiIzdelkov/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipIzdelka = await _context.TipiIzdelkov
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipIzdelka == null)
            {
                return NotFound();
            }

            return View(tipIzdelka);
        }

        // POST: TipiIzdelkov/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipIzdelka = await _context.TipiIzdelkov.FindAsync(id);
            _context.TipiIzdelkov.Remove(tipIzdelka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipIzdelkaExists(int id)
        {
            return _context.TipiIzdelkov.Any(e => e.ID == id);
        }
    }
}
