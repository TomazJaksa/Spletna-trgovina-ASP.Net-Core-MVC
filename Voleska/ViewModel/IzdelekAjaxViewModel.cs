using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.ViewModel
{
    public class IzdelekAjaxViewModel
    { 
            public int ID { get; set; }
            public int Kolicina { get; set; }
            public decimal Znesek { get; set; }
            public decimal SkupniZnesek { get; set; }

            public int SkupnaKolicina { get; set; }
        
    }
}
