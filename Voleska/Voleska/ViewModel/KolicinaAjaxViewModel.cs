using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.ViewModel
{
    public class KolicinaAjaxViewModel
    { 
    
            public int Kolicina { get; set; }
            public int ID { get; set; }
        
    }

    public class ZnesekAjaxViewModel { 
        public decimal Znesek { get; set; }
        public int ID { get; set; }
    }

    public class SkupniZnesekAjaxViewModel { 
        public decimal SkupniZnesek { get; set; }
    }

    public class SkupnaKolicinaAjaxViewModel { 
        public int SkupnaKolicina { get; set; }
    }
}
