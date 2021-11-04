using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Dopisovanje
    {
        // Primarni ključ
        public int ID { get; set; }

        public DateTime Dodan { get; set; }

        //Tuji ključi - povezave s tabelama Pogovor in Sporočilo
        public int PogovorID { get; set; }
        public int SporociloID { get; set; }

        public Pogovor Pogovor { get; set; }
        public Sporocilo Sporocilo { get; set; }
    }
}
