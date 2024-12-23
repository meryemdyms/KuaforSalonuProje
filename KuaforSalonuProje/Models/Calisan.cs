using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KuaforSalonuProje.Models
{
    public class Calisan
    {
        [Key]
        public int CalisanId { get; set; }

        [StringLength(50)]
        public string CalisanAdi { get; set; }

        [StringLength(50)]
        public string CalisanSoyadi { get; set; }

        [StringLength(50)]
        public string UzmanlikAlani { get; set; }


        public ICollection<Randevu> Randevular { get; set; }


    }
}
