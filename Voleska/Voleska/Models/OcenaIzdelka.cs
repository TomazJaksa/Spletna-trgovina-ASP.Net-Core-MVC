using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class OcenaIzdelka
    {
        
        public int ID { get; set; }
        public string Komentar { get; set; }
        public byte Ocena { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Dodan { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Posodobljen { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ApplicationUserID { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? IzdelekID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public Izdelek Izdelek { get; set; }

    }
}