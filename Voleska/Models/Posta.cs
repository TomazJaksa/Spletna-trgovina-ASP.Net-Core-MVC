using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Posta
    {
       // [DatabaseGenerated(DatabaseGeneratedOption.None)]
       // Primarni ključ
        public int ID { get; set; }

        public string? PostnaStevilka { get; set; } // Vrednost je lahko null
        public string? Kraj { get; set; } // Vrednost je lahko null

        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        // Povezava s tabelo Naslov (one-to-many). Vsaka pošta ima lahko več naslovov, vsak naslov pa ima eno pošto.
        public ICollection<Naslov> Naslovi { get; set; }
    }
}
