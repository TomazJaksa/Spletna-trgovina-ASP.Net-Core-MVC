using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Blog
    {
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
        public ICollection<Komentar> Komentarji { get; set; }
        public ICollection<LajkanjeBlogov> LajkaniBlogi { get; set; }
    }
}
