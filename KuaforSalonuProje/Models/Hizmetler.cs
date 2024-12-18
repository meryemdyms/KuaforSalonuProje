namespace KuaforSalonuProje.Models
{
    public class Hizmetler
    {
        public int HizmetlerId { get; set; }  // Her hizmet için benzersiz bir ID
        public string HizmetAdi { get; set; } // Hizmetin adı (örneğin, Saç Kesimi, Saç Boyama vb.)
        public string Aciklama { get; set; }  // Hizmet hakkında açıklama
        public decimal Fiyat { get; set; }    // Hizmetin fiyatı
        public string Resim { get; set; }     // Hizmete ait bir görsel (isteğe bağlı)

        // Hizmetin sunulduğu salon ile ilişkilendirme
        public int SalonId { get; set; }      // SalonId ile salonu ilişkilendiriyoruz
        public Salon Salon { get; set; }      // Salon nesnesi ile ilişkiyi belirtiyoruz
    }
}
