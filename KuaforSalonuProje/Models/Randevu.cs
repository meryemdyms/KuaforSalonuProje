using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuaforSalonuProje.Models
{
    public class Randevu
    {
        public int RandevuId { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public DateTime RandevuSaati { get; set; }
        public string IslemAdi { get; set; }
        public double ucret { get; set; }

        // Çalışan Foreign Key ve Navigation Property
        public int CalisanId { get; set; }
        public virtual Calisan Calisan { get; set; }

        // Kullanıcı Foreign Key ve Navigation Property
        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }

        // Hizmet Foreign Key ve Navigation Property
        public int HizmetId { get; set; }
        public virtual Hizmet Hizmet { get; set; }

        // Yeni eklenen Durum alanı
        public string Durum { get; set; } = "Onay Bekliyor"; // Varsayılan olarak "Onay Bekliyor"

    }
}
