using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        PrenosPodatkovRoleVC
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
           
            var blog = await _context.Blogi
                .Include(i => i.LajkaniBlogi)
                .Include(i => i.Komentarji)
                    .ThenInclude(i => i.ApplicationUser)
                .Include(i => i.Komentarji)
                    .ThenInclude(i => i.LajkaniKomentarji)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == blogId);

            ViewData["NovKomentar"] = komentarId;
            
            return View(blog);


        }
    }

    [ViewComponent(Name = nameof(ViewComponents.TooltipKomentarVC))]
    public class TooltipKomentarViewComponent : ViewComponent
    {

        public TooltipKomentarViewComponent()
        {
           
        }

        public async Task<IViewComponentResult> InvokeAsync(int komentarId, int blogId)
        {

            LajkAjaxViewModel lajkAjax = new LajkAjaxViewModel()
            {

                KomentarID = komentarId,
                ID = blogId
            };

            return View(lajkAjax);


        }
    }

    [ViewComponent(Name = nameof(ViewComponents.VnosKomentarjaVC))]
    public class VnosKomentarjaViewComponent : ViewComponent
    {
    
        public VnosKomentarjaViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
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

}
