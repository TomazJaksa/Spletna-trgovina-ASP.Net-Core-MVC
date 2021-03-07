using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class Transakcija
    {
        
        public int ID { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SkupniZnesek { get; set; }

        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        public bool Zakljuceno { get; set; }

        public bool Odposlano { get; set; }

        //Tuji ključi - povezava s tabelo Uporabnik
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<Narocilo> Narocila { get; set; }
    }
}
