using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class Komentar
    {
        public int ID { get; set; }
        public string Vsebina { get; set; }
        public int SteviloLike { get; set; }
        public int SteviloDislike { get; set; }
        
        
        public System.DateTime Dodan { get; set; }

        
        public System.DateTime Posodobljen { get; set; }

        //Tuji ključi - povezavi s tabelami Blog in Uporabnik
        public string ApplicationUserID { get; set; }
        public int BlogID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public Blog Blog { get; set; }

        public ICollection<LajkanjeKomentarjev> LajkaniKomentarji { get; set; }
    }
}
