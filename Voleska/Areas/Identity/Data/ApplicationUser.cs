using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Models;

namespace Voleska.Areas.Identity.Data
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Ime { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Priimek { get; set; }

        public bool Aktiven { get; set; }

        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        // Tuji ključ - povezava s tabelo naslov
        public int NaslovID { get; set; }
        public virtual Naslov Naslov { get; set; }

        // Vsak uporabnik ima lahko eno ali več transakcij, komentarjev, všečkov na komentarje, všečkov na bloge
        // ... eno ali več ocen na izdelke in eno ali več vlog (one-to-many) povezava z vsemi spodnjimi tabelami
        public virtual ICollection<Transakcija> Transakcije { get; set; }
        public virtual ICollection<Komentar> Komentarji { get; set; }
        public virtual ICollection<LajkanjeKomentarjev> LajkaniKomentarji { get; set; }
        public virtual ICollection<LajkanjeBlogov> LajkaniBlogi { get; set; }

        public virtual ICollection<OcenaIzdelka> OceneIzdelkov { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }




    }
}
