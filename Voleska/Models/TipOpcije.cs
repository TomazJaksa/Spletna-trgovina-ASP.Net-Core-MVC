using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class TipOpcije
    {
        // Primarni ključ
        public int ID { get; set; }

        public string Ime { get; set; }

        // Povezava med Tipi opcij in opcijami (one-to-many). Vsak tip opcije ima svojo zbirko opcij, ki mu pripadajo.
        public ICollection<Opcija> Opcije { get; set; }
    }
}
