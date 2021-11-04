using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class LajkanjeKomentarjev
    {
        // Primarni ključ
        public int ID { get; set; }

        public bool? Lajk { get; set; } // Všeček. Vrednost je lahko null.

        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }
        
        // Tuji ključi. Povezava s tabelama uporabnik in komentar. 
        // Vsak všeček pripada natanko enemu uporabniku in enemu komentarju.
        public string ApplicationUserID { get; set; }
        public int? KomentarID { get; set; } // komentar je lahko null

        public virtual ApplicationUser ApplicationUser { get; set; }
        public Komentar Komentar { get; set; }

    }
}
