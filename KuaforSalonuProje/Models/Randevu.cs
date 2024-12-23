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
        public double ucret {  get; set; }

        //calisanıd
        public int CalisanId { get; set; }
        public virtual Calisan Calisan { get; set; }

        //kullaniciıd
        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }


    }
}
