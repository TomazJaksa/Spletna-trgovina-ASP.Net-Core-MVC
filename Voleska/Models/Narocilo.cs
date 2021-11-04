using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Narocilo
    {
        // Primarni ključ
        public int ID { get; set; }

        public int Kolicina { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Znesek { get; set; }

        public string Opombe { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        // Tuji ključi - povezave s tabelami Transakcija, izdelek, opcija in material.
        // Vsakemu naročilu pripada natanko eden izdelek, eden material, ena opcija in ena transakcija.
        public int IzdelekID { get; set; }
        public int TransakcijaID { get; set; }
        public int? OpcijaID { get; set; }
        public int? MaterialID { get; set; }

        public Izdelek Izdelek { get; set; }
        public Transakcija Transakcija { get; set; }
        public Opcija Opcija { get; set; }
        public Material Material { get; set; }



    }
}
