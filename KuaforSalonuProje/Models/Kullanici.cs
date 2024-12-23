using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KuaforSalonuProje.Models
{
    public class Kullanici
    {
        [Key]
        public int KullaniciId { get; set; }

        [StringLength(50)]
        public string Adi { get; set; }

        [StringLength(50)]
        public string Soyadi { get; set; }

        [StringLength(50)]
        public string KullaniciAdi { get; set; }
        [StringLength(20)]
        public string Sifre { get; set; }

        public ICollection<Randevu> Randevular { get; set; }




    }
}
