using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voleska.Models;

namespace Voleska.ViewModel
{
        public class TipIzdelkaViewModel
        {
            public int ID { get; set; }
            public string Ime { get; set; }
            public IFormFile Slika { get; set; }
            public ICollection<Izdelek> Izdelki { get; set; }
        }
    
}
