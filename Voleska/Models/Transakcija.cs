using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class Transakcija
    {
        // Primarni ključ
        public int ID { get; set; }

        [DisplayName("Skupni znesek")]
        [DataType(DataType.Currency)] // Anotacija za valute
        [Column(TypeName = "decimal(18,2)")]
        public decimal SkupniZnesek { get; set; }

        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        public bool Zakljuceno { get; set; }
        public bool Odposlano { get; set; }

        // Tuji ključi - povezava s tabelo Uporabnik
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        // Povezava s tabelo Naročilo (one-to-many). Vsaki transakciji pripada zbirka naročil.
        public ICollection<Narocilo> Narocila { get; set; }
    }
}
