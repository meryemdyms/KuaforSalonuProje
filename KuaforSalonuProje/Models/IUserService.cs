namespace KuaforSalonuProje.Models
{
    public interface IUserService
    {
        Kullanici ValidateUser(string kullaniciAd, string sifre);
    }
}
