using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KuaforSalonuProje.Models
{
    public class Salon
    {
        public int SalonId { get; set; } // Primary Key
        public string SalonAdi { get; set; } // Salon adı

        // Navigation Property
        public ICollection<Hizmet> Hizmetler { get; set; } // Salona bağlı hizmetler
        public ICollection<Randevu> Randevular { get; set; } // Salona bağlı randevular
    }

}
