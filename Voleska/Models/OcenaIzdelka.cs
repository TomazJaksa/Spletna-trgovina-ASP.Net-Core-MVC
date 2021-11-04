using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voleska.Areas.Identity.Data;

namespace Voleska.Models
{
    public class OcenaIzdelka
    {
        // Primarni ključ
        public int ID { get; set; }

        public string Komentar { get; set; }
        public byte Ocena { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)] // Določimo format, kako naj se datum zapisuje v bazo
        [DataType(DataType.Date)]
        public DateTime Dodan { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)] // Določimo format, kako naj se datum zapisuje v bazo
        [DataType(DataType.Date)]
        public DateTime Posodobljen { get; set; }


        // Tuji ključi - povezava s tabelami ApplicationUser (uporabnik) in Izdelek
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Ne dovolimo bazi, da vrednost kreira sama, ampak jo določimo mi v kodi
        public string ApplicationUserID { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? IzdelekID { get; set; } // Ta lastnost ima lahko tudi vrednost NULL

        public virtual ApplicationUser ApplicationUser { get; set; }
        public Izdelek Izdelek { get; set; }

    }
}