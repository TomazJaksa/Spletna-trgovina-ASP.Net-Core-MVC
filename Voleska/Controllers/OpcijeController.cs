﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
    public class OpcijeController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public OpcijeController(Data.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: Opcije
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.Opcije.Include(o => o.Izdelek).Include(o => o.TipOpcije);
            return View(await storeContext.ToListAsync());
        }

        // GET: Opcije/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opcija = await _context.Opcije
                .Include(o => o.Izdelek)
                .Include(o => o.TipOpcije)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opcija == null)
            {
                return NotFound();
            }

            return View(opcija);
        }

        // GET: Opcije/Create
        public IActionResult Create(int izdelekId)
        {
            ViewBag.IzdelekId = izdelekId;
            ViewData["TipOpcijeID"] = new SelectList(_context.TipiOpcij, "ID", "Ime");
            return View();
        }

        // POST: Opcije/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ime,Slika,Zaloga,Dodan,Posodobljen,IzdelekID,TipOpcijeID")] OpcijaViewModel vm, int izdelekId)
        {
            Opcija opcija = new Opcija();
            if (ModelState.IsValid)
            {
                opcija.Ime = vm.Ime;
                opcija.IzdelekID = izdelekId;
                string imeSlike = UploadFile(vm);
                opcija.Zaloga = vm.Zaloga;
                opcija.Slika = imeSlike;
                opcija.Dodan = DateTime.Now;
                opcija.Posodobljen = DateTime.Now;
                opcija.TipOpcijeID = vm.TipOpcijeID;
                _context.Add(opcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
            return View(opcija);
        }

        private string UploadFile(OpcijaViewModel vm)
        {
            string imeSlike = null;
            if (vm.Slika != null) {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "C:/Users/CRIMSON/source/repos/Voleska/Voleska/wwwroot/slike");
                imeSlike = Guid.NewGuid().ToString() + "-" + vm.Slika.FileName;
                string filePath = Path.Combine(uploadDir, imeSlike);

                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    vm.Slika.CopyTo(fileStream);
                }

                var photo_thumbnail = "C:/Users/CRIMSON/source/repos/Voleska/Voleska/wwwroot/thumbnail/" + imeSlike;
                ResizeImage(vm.Slika, photo_thumbnail, 100);

                var katalogSlika = "C:/Users/CRIMSON/source/repos/Voleska/Voleska/wwwroot/katalogSlike/" + imeSlike;
                ResizeImage(vm.Slika, katalogSlika, 378);

            }



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


        // GET: Opcije/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Opcija opcija = await _context.Opcije
                   .FromSqlRaw(@"SELECT * FROM Opcija WHERE  ID = {0}", id)
                   .AsNoTracking()
                   .FirstOrDefaultAsync();

            OpcijaEditViewModel opcijaEditViewModel = new OpcijaEditViewModel
            {

                ID = opcija.ID,
                Ime = opcija.Ime,
                ObstojecaSlika = opcija.Slika,
                Zaloga = opcija.Zaloga,
                Dodan = opcija.Dodan,
                Posodobljen = opcija.Posodobljen,
                IzdelekID = opcija.IzdelekID,
                TipOpcijeID = opcija.TipOpcijeID 

            };
            ViewData["izdelekId"] = opcija.IzdelekID;
            return View(opcijaEditViewModel);

        }

        // POST: Opcije/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Ime,Slika,Zaloga")] OpcijaViewModel vm)
        {
            var opcija = await _context.Opcije
                    .FromSqlRaw(@"SELECT * FROM Opcija WHERE  ID = {0}", id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            if (id != opcija.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    opcija.Ime = vm.Ime;
                    string imeSlike = UploadFile(vm);

                    if (imeSlike != null)
                    {
                        opcija.Slika = imeSlike;
                    }
                    
                    opcija.Zaloga = vm.Zaloga;
                    opcija.Posodobljen = DateTime.Now;
                 
                    _context.Update(opcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpcijaExists(opcija.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Izdelki", new { @id = opcija.IzdelekID });
            }
            ViewData["IzdelekID"] = new SelectList(_context.Izdelki, "ID", "ID", opcija.IzdelekID);
            ViewData["TipOpcijeID"] = new SelectList(_context.TipiOpcij, "ID", "ID", opcija.TipOpcijeID);
            return View(opcija);
        }

        // GET: Opcije/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opcija = await _context.Opcije
                .Include(o => o.Izdelek)
                .Include(o => o.TipOpcije)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opcija == null)
            {
                return NotFound();
            }

            return View(opcija);
        }

        // POST: Opcije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opcija = await _context.Opcije.FindAsync(id);
            _context.Opcije.Remove(opcija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpcijaExists(int id)
        {
            return _context.Opcije.Any(e => e.ID == id);
        }
    }
}