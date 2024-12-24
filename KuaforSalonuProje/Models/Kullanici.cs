using System.ComponentModel.DataAnnotations;

public class Kullanici
{
    [Key]
    public int KullaniciId { get; set; }

    [Required]
    [StringLength(50)]
    public string Adi { get; set; }

    [Required]
    [StringLength(50)]
    public string Soyadi { get; set; }

    [Required]
    [StringLength(50)]
    public string KullaniciAdi { get; set; }

    [Required]
    [StringLength(20)]
    public string Sifre { get; set; }
}
