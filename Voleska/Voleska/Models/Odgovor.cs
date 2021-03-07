using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Odgovor
    {
        public int ID { get; set; }

        public System.DateTime Dodan { get; set; }

        /* To polje predstavlja komentar, na katerega je nekdo odgovoril */
        [ForeignKey("TaKomentar")]
        public int? TaKomentarID { get; set; }

        /* To polje predstavlja odgovor na zgornji komentar. Seveda je odgovor po tipu tudi komentar.*/
        [ForeignKey("OdgovorNaTaKomentar")]
        public int? OdgovorNaTaKomentarID { get; set; }

        //Tuji ključi komentar/odgovor
        public virtual Komentar TaKomentar { get; set; }
        public virtual Komentar OdgovorNaTaKomentar { get; set; }
        
    }
}
