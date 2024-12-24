using System.ComponentModel.DataAnnotations;

namespace KuaforSalonuProje.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string KullaniciAd { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        public bool BeniHatirla { get; set; } // "Beni hatırla" seçeneği
    }
}
