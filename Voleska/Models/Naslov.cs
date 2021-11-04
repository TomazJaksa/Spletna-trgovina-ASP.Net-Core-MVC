using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class Naslov
    {
        // Primarni ključ
        public int ID { get; set; }

        public string? Ulica { get; set; } // Vrednost je lahko null
        public string? HisnaStevilka { get; set; } // Vrednost je lahko null

        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        //Tuji ključi - povezava s tabelo Posta
        public int PostaID { get; set; }
        public Posta Posta { get; set; }

        //Tuji ključ -uporabnik
        public string ApplicationUserID { get; set; }

        // Vsak naslov ima skupino uporabnikov, ki spadajo pod ta naslov
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}
