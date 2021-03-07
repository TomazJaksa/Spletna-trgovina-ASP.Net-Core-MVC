using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class Naslov
    {
        public int ID { get; set; }

        public string ApplicationUserID { get; set; }

        public string? Ulica { get; set; }
        public string? HisnaStevilka { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        //Tuji ključi - povezava s tabelo Posta
        public int PostaID { get; set; }
        public Posta Posta { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}
