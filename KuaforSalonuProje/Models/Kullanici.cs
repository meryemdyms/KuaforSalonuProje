namespace KuaforSalonuProje.Models
{
    public class Kullanici
    {
        public int KullaniciId { get; set; }  // Kullanıcı ID'si
        public string Adi { get; set; }  // Kullanıcı adı
        public string Soyadi { get; set; }  // Kullanıcı soyadı
        public string KullanıcıAd { get; set; }  // Kullanıcı e-posta adresi
       

        public string Sifre { get; set; }
        public string Rol { get; set; }


        // Kullanıcının randevuları
        public ICollection<Randevu> Randevular { get; set; }
        public string KullaniciAd { get; internal set; }
    }
}
