namespace KuaforSalonuProje.Models
{
    public class UserService : IUserService
    {
        private readonly KuaforSalonuContext _context;

        public UserService(KuaforSalonuContext context)
        {
            _context = context;
        }

        public Kullanici ValidateUser(string kullaniciAd, string sifre)
        {
            // Kullanıcı adı ve şifreyi kontrol et
            return _context.Kullanıcılar.SingleOrDefault(u => u.KullanıcıAd == kullaniciAd && u.Sifre == sifre);
        }
    }
}
