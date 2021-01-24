using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Posta
    {
       // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public string? PostnaStevilka { get; set; }
        public string? Kraj { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }
        public ICollection<Naslov> Naslovi { get; set; }
    }
}
