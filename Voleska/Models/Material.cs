using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Material
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Opis { get; set; }

        public ICollection<Izdelek> Izdelki { get; set; }
        public ICollection<Narocilo> Narocila { get; set; }
    }
}
