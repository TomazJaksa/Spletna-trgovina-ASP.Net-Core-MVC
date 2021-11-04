using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Novice
    {
        // Primarni ključ
        public int ID { get; set; }
        
        public string Email { get; set; } // elektronski naslov kamor pošiljamo sporočila
        public string UporabniskoIme { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }
    }
}
