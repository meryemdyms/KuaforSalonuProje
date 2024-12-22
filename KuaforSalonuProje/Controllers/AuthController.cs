using KuaforSalonuProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KuaforSalonuProje.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly KuaforSalonuContext _context;

        public AuthController(UserService userService, KuaforSalonuContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var user = _userService.ValidateUser(login.KullaniciAd, login.Sifre);

            if (user == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre yanlış.");
            }

            if (user.Rol == "Admin")
            {
                // Admin girişinde yönlendirme
                return Ok(new { redirectUrl = "/admin/dashboard" });
            }
            else
            {
                // Kullanıcı girişi
                return Ok(new { redirectUrl = "/user/dashboard" });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            if (_context.Kullanicilar.Any(u => u.KullaniciAd == model.KullaniciAd))
            {
                return BadRequest(new { message = "Bu kullanıcı adı zaten mevcut." });
            }

            // Şifreyi hash'leyin
            string hashedPassword = HashPassword(model.Sifre);

            // Yeni kullanıcı oluşturma
            var newUser = new Kullanici
            {
                Adi = model.Adi,
                Soyadi = model.Soyadi,
                KullaniciAd = model.KullaniciAd,
                Sifre = hashedPassword, // Şifre burada hash edilmiştir
                Rol = "User" // Varsayılan rol
            };

            _context.Kullanicilar.Add(newUser);
            _context.SaveChanges();

            return Ok(new { success = true });
        }

        // Şifreyi hash'leyen yardımcı metod
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
