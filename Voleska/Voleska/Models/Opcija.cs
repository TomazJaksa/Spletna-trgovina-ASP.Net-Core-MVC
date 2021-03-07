using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Opcija
    {
        public int ID { get; set; }
        public string Ime { get; set; }

        public string Slika {get; set;} // vprašaj Boruta za uploadanje slik
        public bool Zaloga { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        //Tuji ključi - povezave s tabelami Izdelek in TipOpcije
        public int IzdelekID { get; set; }

        [DisplayName("Tip opcije")]
        public int TipOpcijeID { get; set; }

        public Izdelek Izdelek { get; set; }
        public TipOpcije TipOpcije { get; set; }
        public ICollection<Narocilo> Narocila { get; set; }



    }
}
