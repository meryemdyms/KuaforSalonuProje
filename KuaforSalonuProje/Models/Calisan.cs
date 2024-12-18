namespace KuaforSalonuProje.Models
{
    public class Calisan
    {
        public int CalisanId { get; set; }  // Çalışanın ID'si
        public string Adi { get; set; }  // Çalışanın adı
        public string Soyadi { get; set; }  // Çalışanın soyadı
        public string UzmanlikAlani { get; set; }  // Çalışanın uzmanlık alanı
        public string Telefon { get; set; }  // Çalışanın telefonu
        public int SalonId { get; set; }  // Hangi salonda çalıştığı (SalonId ile ilişkilendirme)

        // Çalışanın yaptığı randevular
        public ICollection<Randevu> Randevular { get; set; }

        // Salonla ilişkili olması için
        public Salon Salon { get; set; }
    }

}
