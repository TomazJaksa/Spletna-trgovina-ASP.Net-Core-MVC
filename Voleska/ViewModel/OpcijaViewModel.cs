using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Models;

namespace Voleska.ViewModel
{
    public class OpcijaViewModel
    {
        public string Ime { get; set; }

        public IFormFile Slika { get; set; } 
        public bool Zaloga { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        //Tuji ključi - povezave s tabelami Izdelek in TipOpcije
        public int IzdelekID { get; set; }
        public int TipOpcijeID { get; set; }

        public Izdelek Izdelek { get; set; }
        public TipOpcije TipOpcije { get; set; }
    }
}
