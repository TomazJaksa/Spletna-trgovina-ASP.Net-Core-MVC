using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class TipOpcije
    {
        public int ID { get; set; }
        public string Ime { get; set; }

        public ICollection<Opcija> Opcije { get; set; }
    }
}
