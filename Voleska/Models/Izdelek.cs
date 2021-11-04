using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Izdelek
    {
        // Primarni ključ
        public int ID { get; set; }
        
        public string Ime { get; set; }
        public string Opis { get; set; }
        public string Podrobnosti { get; set; }

        [DataType(DataType.Currency)] // Anotacija za valute. 
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)] // Prikaži € zraven izpisa cene. Oz. katerokoli kulturo določimo.
        public decimal Cena { get; set; }

        public bool Izbrisan { get; set; }

        [DisplayName("Povprečna ocena")]
        public int PovprecnaOcena { get; set; }
        public int SteviloProdanih { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        //Tuji ključi - povezave s tabelami Material in TipIzdelka
        public int MaterialID { get; set; }

        [DisplayName("Tip izdelka")]
        public int TipIzdelkaID { get; set; }

        public Material Material {get; set;} // vsak izdelek pripada enemu materialu

        [DisplayName("Tip izdelka")]
        public TipIzdelka TipIzdelka {get; set;} // vsak izdelek ima svoj tip

        //Povezava s tabelo Opcija, Narocila in OceneIzdelkov (one-to-many) En Izdelek ima več opcij, več naročil in več ocen
        public ICollection<Opcija> Opcije { get; set; } 
        public ICollection<Narocilo> Narocila { get; set; }

        public virtual ICollection<OcenaIzdelka> OceneIzdelkov { get; set; }

    }
}
