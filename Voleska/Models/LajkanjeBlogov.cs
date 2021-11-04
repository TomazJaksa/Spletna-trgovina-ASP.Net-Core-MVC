using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class LajkanjeBlogov
    {
        // Primarni ključ
        public int ID { get; set; }

        public bool? Lajk { get; set; } // Všeček

        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        
        // Tuji ključi - Povezava s tabelama blog in uporabnik
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Preprečimo avtomatsko generiranje ID
        public string ApplicationUserID { get; set; }

        
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Preprečimo avtomatsko generiranje ID
        public int? BlogID { get; set; } // Vrednost je lahko null

        public virtual ApplicationUser ApplicationUser { get; set; }
        public Blog Blog { get; set; }

    }
}
