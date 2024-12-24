using System.ComponentModel.DataAnnotations;

namespace KuaforSalonuProje.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad gerekli.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gerekli.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta gerekli.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gerekli.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrar gerekli.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}
