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

        [Required(ErrorMessage = "Çalışan Adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Çalışan Adı en fazla 50 karakter olabilir.")]
        public string CalisanAdi { get; set; }

        [Required(ErrorMessage = "Çalışan Soyadı zorunludur.")]
        [StringLength(50, ErrorMessage = "Çalışan Soyadı en fazla 50 karakter olabilir.")]
        public string CalisanSoyadi { get; set; }

        [StringLength(50, ErrorMessage = "Uzmanlık Alanı en fazla 50 karakter olabilir.")]
        public string UzmanlikAlani { get; set; }

        public ICollection<Randevu> Randevular { get; set; }


    }
}
