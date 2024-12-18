namespace KuaforSalonuProje.Models
{
    public class Salon
    {
        public int SalonId { get; set; }
        public string SalonAdı { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public DateTime CalismaBaslangicSaati { get; set; }
        public DateTime CalismaBitisSaati { get; set; }

        // Bir salonda birden fazla çalışan olabilir.
        public ICollection<Calisan> Calisanlar { get; set; }

        // Salonun sunduğu hizmetler
        public ICollection<Hizmetler> Hizmetler { get; set; }

        // Salonun randevuları
        public ICollection<Randevu> Randevular { get; set; }

    }
}
