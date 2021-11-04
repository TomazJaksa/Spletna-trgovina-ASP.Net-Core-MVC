using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Models;

namespace Voleska.ViewModel
{
    public class SporociloViewModel
    {
        public int PogovorID { get; set; }
        public string Ime { get; set; }
        public Sporocilo Sporocilo { get; set; }
        public DateTime Dodan { get; set; }
    }
}
