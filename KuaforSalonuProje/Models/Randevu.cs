namespace KuaforSalonuProje.Models
{
    public class Randevu
    {
        public int RandevuId { get; set; }  // Randevu ID'si
        public DateTime RandevuSaati { get; set; }  // Randevu saati
        public string IslemAdi { get; set; }  // Yapılacak işlem (saç kesimi, boyama vb.)
        public decimal Ucret { get; set; }  // İşlemin ücreti

        // Çalışan ile ilişki
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        // Salon ile ilişki
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        // Kullanıcı ile ilişki
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        public DateTime RandevuZamani { get; set; }
    }
}