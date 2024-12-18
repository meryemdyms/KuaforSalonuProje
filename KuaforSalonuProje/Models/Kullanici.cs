namespace KuaforSalonuProje.Models
{
    public class Kullanici
    {
        public int KullaniciId { get; set; }  // Kullanıcı ID'si
        public string Adi { get; set; }  // Kullanıcı adı
        public string Soyadi { get; set; }  // Kullanıcı soyadı
        public string Email { get; set; }  // Kullanıcı e-posta adresi
        public string Telefon { get; set; }  // Kullanıcı telefonu

        public string Sifre { get; set; }
        public string Rol { get; set; }


        // Kullanıcının randevuları
        public ICollection<Randevu> Randevular { get; set; }
    }
}
