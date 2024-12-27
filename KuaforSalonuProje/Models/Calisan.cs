using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KuaforSalonuProje.Models
{
    public class Calisan
    {
        [Key]
        public int CalisanId { get; set; }

        [Required(ErrorMessage = "Çalışan adı gereklidir.")]
        [StringLength(50)]
        public string CalisanAdi { get; set; }

        [Required(ErrorMessage = "Çalışan soyadı gereklidir.")]
        [StringLength(50)]
        public string CalisanSoyadi { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı gereklidir.")]
        [StringLength(50)]
        public string UzmanlikAlani { get; set; }

        // Opsiyonel Randevular
        public ICollection<Randevu>? Randevular { get; set; }
    }
}
