using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class TipIzdelka
    {
        // Primarni ključ
        public int ID { get; set; }

        public string Ime { get; set; }
        public string Slika { get; set; }

        // Povezava med izdelkom in TipiIzdelka (one-to-many). Vsakemu tipu izdelka pripada zbirka izdelkov.
        public ICollection<Izdelek> Izdelki { get; set; }
    }
}
