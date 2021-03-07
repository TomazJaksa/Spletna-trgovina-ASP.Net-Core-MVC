using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class LajkanjeBlogov
    {
        public int ID { get; set; }
        public bool? Lajk { get; set; }
        public DateTime Dodan { get; set; }
        public DateTime Posodobljen { get; set; }

        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ApplicationUserID { get; set; }

        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? BlogID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public Blog Blog { get; set; }

    }
}
