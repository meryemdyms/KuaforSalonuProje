using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuaforSalonuProje.Models
{
    public class Hizmet
    {
        public int HizmetId { get; set; } // Primary Key
        public string HizmetAdi { get; set; } // Hizmet adı

        public decimal Ucret { get; set; } // Ücret alanı

      
    }

}
