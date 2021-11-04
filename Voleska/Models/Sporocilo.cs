using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Models
{
    public class Sporocilo
    {
        // Primarni ključ
        public int ID { get; set; }

        public string Ime { get; set; }
        public string? Priimek { get; set; }

        [Required(ErrorMessage = "E-naslov je obvezen!")]
        [EmailAddress(ErrorMessage = "Uporabite format: lep.primer@hotmail.com")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Tema { get; set; }
        public string Vsebina { get; set; }
        public bool Odprto { get; set; }
        public bool Odgovorjeno { get; set; }

        // Povezava s tabelo Dopisovnje. 
        public ICollection<Dopisovanje> Dopisovanja { get; set; }

    }
}
