using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Models;

namespace Voleska.ViewModel
{
    public class IzdelkiViewModel
    {
        public PaginatedList<Izdelek> Izdelki { get; set; }
        public SelectList TipiIzdelka { get; set; }
        public string IzbranTipIzdelka { get; set; }

        public string SearchString { get; set; }
    }
}
