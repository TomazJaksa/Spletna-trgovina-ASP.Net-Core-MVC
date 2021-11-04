using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class Komentar
    {
        // Primarni ključ
        public int ID { get; set; }

        public string Vsebina { get; set; }
        public int SteviloLike { get; set; }
        public int SteviloDislike { get; set; }
        
        public int Nivo { get; set; } // Na podlagi te lastnosti izvemo ali je komentar začetni komentar, odgovor ali odgovor na odgovor. Nivoji gredo od 1 navzgor.

        public System.DateTime Dodan { get; set; }

        
        public System.DateTime Posodobljen { get; set; }

        // Tuji ključi - povezavi s tabelama Blog in Uporabnik.
        // Vsak komentar pripada natanko enemu uporabniku in enemu blogu.
        // Vsak blog in uporabnik pa lahko imata več komentarjev
        public string ApplicationUserID { get; set; }
        
        public int BlogID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public Blog Blog { get; set; }


        // Povezava s tabelo LajkanjeKomentarjev. Vsak komentar ima svojo zbirko všečkov (one-to-many).
        public ICollection<LajkanjeKomentarjev> LajkaniKomentarji { get; set; }

        // Spodnji dve lastnosti uporabljamo, za tabelo Odgovor, ki je vmesna tabela med dvema komentarjema.
        [InverseProperty("TaKomentar")] // Predstavlja nek komentar
        public virtual ICollection<Odgovor> KomentarjiZOdgovoromi { get; set; }

        [InverseProperty("OdgovorNaTaKomentar")] // Predstavlja odgovor na ta komentar
        public virtual ICollection<Odgovor> OdgovoriNaTaKomentar { get; set; }
    }
}
