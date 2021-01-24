﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class TipIzdelka
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Slika { get; set; }
        public ICollection<Izdelek> Izdelki { get; set; }
    }
}