using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class LajkanjeKomentarjev
    {
        public int ID { get; set; }
        public bool? Lajk { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }
        public string ApplicationUserID { get; set; }
        public int? KomentarID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public Komentar Komentar { get; set; }

    }
}
