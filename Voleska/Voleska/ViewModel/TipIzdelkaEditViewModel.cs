using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Voleska.ViewModel
{
    public class TipIzdelkaEditViewModel : TipIzdelkaViewModel
    {
        public int ID { get; set; }
        
     
        public string ObstojecaSlika { get; set; }
    }
}
