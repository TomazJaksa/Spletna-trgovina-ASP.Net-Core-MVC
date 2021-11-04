using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal Znesek { get; set; }
        public int ID { get; set; }
    }

    public class SkupniZnesekAjaxViewModel {
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal SkupniZnesek { get; set; }
    }

    public class SkupnaKolicinaAjaxViewModel { 
        public int SkupnaKolicina { get; set; }
    }
}
