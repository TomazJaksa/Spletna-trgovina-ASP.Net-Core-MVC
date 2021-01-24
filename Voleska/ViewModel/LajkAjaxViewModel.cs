using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.ViewModel
{
    public class LajkAjaxViewModel
    {
        public int KomentarID {get; set;}

        public int ID { get; set; }
        public string Like { get; set; }
        public string Dislike { get; set; }

        public int SteviloLike { get; set; }

        public int SteviloDislike { get; set; }

    }
}
