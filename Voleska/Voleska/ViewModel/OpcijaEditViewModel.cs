using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Voleska.ViewModel
{
    public class OpcijaEditViewModel : OpcijaViewModel
    {
        public int ID { get; set; }


        public string ObstojecaSlika { get; set; }
    }
}
