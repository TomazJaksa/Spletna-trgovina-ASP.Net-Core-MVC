using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Blog
    {
        // Primarni ključ
        public int ID { get; set; }

        public string Naslov { get; set; }
        public string Slika { get; set; }
        public string Povzetek { get; set; }

        [DisplayName("Članek")]
        public string Clanek { get; set; }
        public string Bloger { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        public int SteviloLike { get; set; }
        public int SteviloDislike { get; set; }
        
        // Povezava s tabelama Komentarji in LajkaniBlogi (one-to-many). 
        // Vsak Blog ima več komentarjev, vsak komentar pa pripada natanko enemu blogu.
        // Vsak blog ima več všečkov, vsak všeček pa ima natanko en blog.
        public ICollection<Komentar> Komentarji { get; set; }
        public ICollection<LajkanjeBlogov> LajkaniBlogi { get; set; }

    }
}
