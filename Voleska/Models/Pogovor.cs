using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Pogovor
    {
        // Primarni ključ
        public int ID { get; set; }
  
        public bool Izbrisan { get; set; }
        
        // Povezava s tabelo Dopisovanja (one-to-many). 
        // Pogovoro ima zbirko dopisovanj.
        public ICollection<Dopisovanje> Dopisovanja { get; set; }
        
    }
}
