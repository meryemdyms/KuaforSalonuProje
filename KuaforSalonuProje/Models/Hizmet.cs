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
        [Key]
        public int HizmetId { get; set; }

        [StringLength(50)]
        public string HizmetAdi { get; set; }
        public double Ucret { get; set; }
        public string Sure { get; set; }


    }
}
