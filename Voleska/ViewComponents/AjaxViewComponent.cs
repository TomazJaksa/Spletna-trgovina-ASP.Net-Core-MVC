using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;
using Voleska.Models;
using Voleska.ViewModel;

namespace Voleska.ViewComponents
{
   

    public enum ViewComponents
    {
        PrenosPodatkovVC,
        PrenosPodatkov2VC,
        KolicinaVC,
        SkupnaKolicinaVC,
        ZnesekVC,
        SkupniZnesekVC,
        LajkVC,
        LajkKomentarVC,
        PrenosPodatkovLajkVC,
        PrenosPodatkovLajkKomentarVC,
        SteviloDislikeVC,
        SteviloLikeVC,
        SteviloDislikeKomentarVC,
        SteviloLikeKomentarVC,
        SeznamKomentarjevVC,
        PrenosPodatkovKomentarVC,
        VnosKomentarjaVC,
        IzbrisiKomentarVC,
        TooltipKomentarVC,
        VlogeVC,
        PrenosPodatkovRoleVC,
        OdgovoriVC,
        SteviloOdgovorovVC,
        OpombaVC,
        NarociloVC,
        SteviloSporocilVC,
        SeznamSporocilVC
    }

    [ViewComponent(Name = nameof(ViewComponents.PrenosPodatkovVC))]
    public class PrenosPodatkovViewComponent : ViewComponent
    {
        public PrenosPodatkovViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int kolicina, int narociloId, decimal znesek, decimal skupniZnesek, int skupnaKolicina)
        {
            IzdelekAjaxViewModel izdelekAjax = new IzdelekAjaxViewModel()
            {
                Kolicina = kolicina,
                ID = narociloId,
                SkupnaKolicina = skupnaKolicina,
                Znesek = znesek,
                SkupniZnesek = skupniZnesek
            };

            return View(izdelekAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.PrenosPodatkov2VC))]
    public class PrenosPodatkov2ViewComponent : ViewComponent
    {
        public PrenosPodatkov2ViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int kolicina, int narociloId, decimal znesek, decimal skupniZnesek, int skupnaKolicina)
        {
            IzdelekAjaxViewModel izdelekAjax = new IzdelekAjaxViewModel()
            {
                Kolicina = kolicina,
                ID = narociloId,
                SkupnaKolicina = skupnaKolicina,
                Znesek = znesek,
                SkupniZnesek = skupniZnesek
            };

            return View(izdelekAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.KolicinaVC))]
    public class KolicinaViewComponent : ViewComponent
    {
        public KolicinaViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int kolicina, int narociloId)
        {
            KolicinaAjaxViewModel kolicinaAjax = new KolicinaAjaxViewModel()
            {
                Kolicina = kolicina,
                ID = narociloId
            };

            return View(kolicinaAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.SkupnaKolicinaVC))]
    public class SkupnaKolicinaViewComponent : ViewComponent
    {
        public SkupnaKolicinaViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int skupnaKolicina)
        {
            SkupnaKolicinaAjaxViewModel skupnaKolicinaAjax = new SkupnaKolicinaAjaxViewModel() {  SkupnaKolicina = skupnaKolicina };

            return View(skupnaKolicinaAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.ZnesekVC))]
    public class ZnesekViewComponent : ViewComponent
    {
        public ZnesekViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(decimal znesek, int narociloId)
        {

            ZnesekAjaxViewModel znesekAjax = new ZnesekAjaxViewModel()
            {
                Znesek = znesek,
                ID = narociloId
            };
            
            return View(znesekAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.SkupniZnesekVC))]
    public class SkupniZnesekViewComponent : ViewComponent
    {
        public SkupniZnesekViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(decimal skupniZnesek)
        {
            SkupniZnesekAjaxViewModel skupniZnesekAjax = new SkupniZnesekAjaxViewModel() {
                SkupniZnesek = skupniZnesek,
            };

            return View(skupniZnesekAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.LajkVC))]
    public class LajkViewComponent : ViewComponent
    {
        public LajkViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int blogId, string like, string dislike, int steviloLike, int steviloDislike)
        {
            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {
                ID = blogId,
                Like = like,
                Dislike = dislike,
                SteviloDislike = steviloDislike,
                SteviloLike = steviloLike
            };

            return View(lajkAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.LajkKomentarVC))]
    public class LajkKomentarViewComponent : ViewComponent
    {
        public LajkKomentarViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId, int blogId, string like, string dislike, int steviloLike, int steviloDislike)
        {
            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {
                KomentarID = komentarId,
                ID = blogId,
                Like = like,
                Dislike = dislike,
                SteviloDislike = steviloDislike,
                SteviloLike = steviloLike
            };

            return View(lajkAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.PrenosPodatkovLajkVC))]
    public class PrenosPodatkovLajkViewComponent : ViewComponent
    {
        public PrenosPodatkovLajkViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId, int blogId, string like, string dislike, int steviloLike, int steviloDislike)
        {
            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {
                KomentarID = komentarId,
                ID = blogId,
                Like = like,
                Dislike = dislike,
                SteviloDislike = steviloDislike,
                SteviloLike = steviloLike
            };

            return View(lajkAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.PrenosPodatkovLajkKomentarVC))]
    public class PrenosPodatkovLajkKomentarViewComponent : ViewComponent
    {
        public PrenosPodatkovLajkKomentarViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId, int blogId, string like, string dislike, int steviloLike, int steviloDislike)
        {
            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {
                KomentarID = komentarId,
                ID = blogId,
                Like = like,
                Dislike = dislike,
                SteviloDislike = steviloDislike,
                SteviloLike = steviloLike
            };

            return View(lajkAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.PrenosPodatkovKomentarVC))]
    public class PrenosPodatkovKomentarViewComponent : ViewComponent
    {
        public PrenosPodatkovKomentarViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId)
        {
            LajkAjaxViewModel komentarAjax = new LajkAjaxViewModel()
            {
                KomentarID = komentarId,
               
            };

            return View(komentarAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.IzbrisiKomentarVC))]
    public class IzbrisiKomentarViewComponent : ViewComponent
    {
        public IzbrisiKomentarViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId)
        {
            LajkAjaxViewModel komentarAjax = new LajkAjaxViewModel()
            {
                KomentarID = komentarId,

            };

            return View(komentarAjax);
        }
    }


    [ViewComponent(Name = nameof(ViewComponents.SteviloLikeVC))]
    public class SteviloLikeViewComponent : ViewComponent
    {
        public SteviloLikeViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int steviloLike)
        {
            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {
                SteviloLike = steviloLike
            };

            return View(lajkAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.SteviloDislikeVC))]
    public class SteviloDislikeViewComponent : ViewComponent
    {
        public SteviloDislikeViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int steviloDislike)
        {
            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {
          
                SteviloDislike = steviloDislike,
            };

            return View(lajkAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.SteviloLikeKomentarVC))]
    public class SteviloLikeKomentarViewComponent : ViewComponent
    {
        public SteviloLikeKomentarViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int steviloLike)
        {
            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {
                SteviloLike = steviloLike
            };

            return View(lajkAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.SteviloDislikeKomentarVC))]
    public class SteviloDislikeKomentarViewComponent : ViewComponent
    {
        public SteviloDislikeKomentarViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int steviloDislike)
        {
            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {

                SteviloDislike = steviloDislike,
            };

            return View(lajkAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.SeznamKomentarjevVC))]
    public class SeznamKomentarjevViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public SeznamKomentarjevViewComponent(Data.ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId, int blogId)
        {
            /*
            var blog = await _context.Blogi
                .Include(i => i.LajkaniBlogi)
                .Include(i => i.Komentarji)
                    .ThenInclude(i => i.ApplicationUser)
                .Include(i => i.Komentarji)
                    .ThenInclude(i => i.LajkaniKomentarji)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == blogId);
            */

            var komentarji = from m in _context.Komentarji
                           select m;

            komentarji = komentarji.Where(s => s.BlogID == blogId);


            komentarji = komentarji.OrderByDescending(s => s.Dodan);

            List<Komentar> ustrezniKomentarji = await komentarji.Include(t => t.ApplicationUser).Include(t => t.LajkaniKomentarji).AsNoTracking()
                .ToListAsync();

            
             
            ViewData["NovKomentar"] = komentarId;
            
            return View(ustrezniKomentarji);


        }
    }

    [ViewComponent(Name = nameof(ViewComponents.TooltipKomentarVC))]
    public class TooltipKomentarViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _context;

        public TooltipKomentarViewComponent(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId, int blogId, int parentId)
        {

            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {

                KomentarID = komentarId,
                ID = blogId
            };


            var komentar = await _context.Komentarji.FindAsync(komentarId);

            ViewData["Nivo"] = komentar.Nivo;
            ViewData["ParentId"] = parentId;
            return View(lajkAjax);


        }
    }

    [ViewComponent(Name = nameof(ViewComponents.VnosKomentarjaVC))]
    public class VnosKomentarjaViewComponent : ViewComponent
    {
    
        public VnosKomentarjaViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string tip)
        {
            if (tip == "sporocilo")
            {
                ViewData["TipKomentarja"] = "sporočilo";
            }
            else {
                ViewData["TipKomentarja"] = "komentar";
            }

            
            return View();
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.VlogeVC))]
    public class VlogeViewComponent : ViewComponent
    {
        private readonly Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> roleManager;

        public VlogeViewComponent(Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> roleManager) {
            this.roleManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string selectedRole, string userId)
        {

            var roles = roleManager.Roles.AsNoTracking().ToList();

            ViewData["SelectedRole"] = selectedRole;
            ViewData["UserId"] = userId;
            return View(roles);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.PrenosPodatkovRoleVC))]
    public class PrenosPodatkovRoleViewComponent : ViewComponent
    {
        public PrenosPodatkovRoleViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(string roleId,string userId)
        {
            RoleAjaxViewModel roleAjax = new RoleAjaxViewModel()
            {
                RoleId = roleId,
                UserId = userId

            };

            return View(roleAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.OdgovoriVC))]
    public class OdgovoriViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public OdgovoriViewComponent(Data.ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId, int blogId, int nivo, bool novOdgovor)
        {
            var odgovori = from m in _context.Odgovori
                              select m;

            
            if (novOdgovor==true)
            {
                odgovori = odgovori.Where(s => s.TaKomentarID == komentarId);
                ViewData["SkrijKomentar"] = "animated fadeInUp";
            }
            else {
                odgovori = odgovori.Where(s => s.TaKomentarID == komentarId );
                ViewData["SkrijKomentar"] = "no-display";
                ViewData["NovOdgovor"] = false;
            }

            

            odgovori = odgovori.OrderByDescending(s => s.Dodan);
            /*
            if (novOdgovor) {
                var last = odgovori.OrderByDescending(s => s.Dodan).FirstOrDefault();
                odgovori = odgovori.Where(x => x.ID == last.ID);
            }
            */
            List<Odgovor> ustrezniOdgovori = await odgovori.Include(t => t.OdgovorNaTaKomentar).Include(t => t.OdgovorNaTaKomentar.ApplicationUser).Include(t => t.OdgovorNaTaKomentar.LajkaniKomentarji).ToListAsync();



            var steviloOdgovorov = from m in _context.Odgovori
                           select m;

            steviloOdgovorov = odgovori.Where(s => s.TaKomentarID == komentarId);

            ViewData["StOdgovorov"] = steviloOdgovorov.Count();
            
            ViewData["KomentarId"] = komentarId;
            ViewData["NaslednjiNivo"] = nivo + 1;
            ViewData["BlogId"] = blogId;
            return View(ustrezniOdgovori);


        }
    }

    [ViewComponent(Name = nameof(ViewComponents.SteviloOdgovorovVC))]
    public class SteviloOdgovorovViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public SteviloOdgovorovViewComponent(Data.ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId)
        {
            var odgovori = from m in _context.Odgovori
                           select m;

            odgovori = odgovori.Where(s => s.TaKomentarID == komentarId);

            /*
            odgovori = odgovori.OrderByDescending(s => s.Dodan);
            */

            List<Odgovor> ustrezniOdgovori = await odgovori.Include(t => t.OdgovorNaTaKomentar).Include(t => t.OdgovorNaTaKomentar.ApplicationUser).Include(t => t.OdgovorNaTaKomentar.LajkaniKomentarji).ToListAsync();

            ViewData["StejOdg"] = odgovori.Count();

            return View(ustrezniOdgovori);


        }
    }

    [ViewComponent(Name = nameof(ViewComponents.OpombaVC))]
    public class OpombaViewComponent : ViewComponent
    {
        public OpombaViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(string opomba, int narociloId, string imeIzdelka, bool blagajna)
        {

            OpombaAjaxViewModel opombaAjax = new OpombaAjaxViewModel()
            {
                NarociloID = narociloId,
                Opomba = opomba,
                ImeIzdelka = imeIzdelka
            };

            ViewData["Blagajna"] = blagajna;
            return View(opombaAjax);
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.NarociloVC))]
    public class NarociloViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _context;

        public NarociloViewComponent(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int narociloId, bool blagajna)
        {

            var narocilo = await _context.Narocila.Include(i => i.Material).Include(i => i.Izdelek).Include(i => i.Opcija).Include(i => i.Transakcija).AsNoTracking().FirstOrDefaultAsync(m => m.ID == narociloId);

            ViewData["Blagajna"] = blagajna;
            return View(narocilo);


        }
    }


    [ViewComponent(Name = nameof(ViewComponents.SteviloSporocilVC))]
    public class SteviloSporocilViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _context;

        public SteviloSporocilViewComponent(Data.ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool obvestilo)
        {
            
           

            var prestejSporocila = from m in _context.Sporocila
                                   select m;

            prestejSporocila = prestejSporocila.Where(s => s.Odprto == false);
            var steviloSporocil = prestejSporocila.Count();

            if (obvestilo == true && steviloSporocil > 0)
            {
                return new HtmlContentViewComponentResult(new HtmlString("<span class='badge exclam badge-warning animated fadeIn delay-1s'><i class='fas fa-envelope'></i></span>"));
            }
            else if (obvestilo == false && steviloSporocil > 0)
            {
                return new HtmlContentViewComponentResult(new HtmlString("<span class=' animated fadeIn slow badge messNo ml-1 badge-warning'><span>" + steviloSporocil + "</span></span>"));
            }
            else {
                return Content(string.Empty);
            }
        }
    }

    [ViewComponent(Name = nameof(ViewComponents.SeznamSporocilVC))]
    public class SeznamSporocilViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public SeznamSporocilViewComponent(Data.ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? sporociloId, int pogovorId)
        {
            
            var pogovor = from m in _context.Dopisovanja
                          select m;



            pogovor = pogovor.Where(m => m.PogovorID == pogovorId).OrderBy(s => s.Dodan);



            

            var model = await pogovor.Include(x => x.Sporocilo).ToListAsync();


            ViewData["NovoSporocilo"] = sporociloId;

            return View(model);


        }
    }



}
