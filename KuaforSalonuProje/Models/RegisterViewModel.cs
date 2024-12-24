using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ad gerekli.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Soyad gerekli.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
    [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir.")]
    public string KullaniciAdi { get; set; }

    [Required(ErrorMessage = "Şifre gerekli.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Şifre tekrar gerekli.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
    public string ConfirmPassword { get; set; }
}
