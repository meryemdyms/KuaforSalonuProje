using KuaforSalonuProje.Models;
using Microsoft.AspNetCore.Mvc;

namespace KuaforSalonuProje.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly KuaforSalonuContext _context;
        private KuaforSalonuContext context;

        public AuthController(UserService userService)
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

            // Yeni kullanıcı oluşturma
            var newUser = new Kullanici
            {
                Adi = model.Adi,
                Soyadi = model.Soyadi,
                KullanıcıAd = model.KullaniciAd,
                Sifre = model.Sifre, // Şifre burada hash edilmelidir
                Rol = "User" // Varsayılan rol
            };

            _context.Kullanıcılar.Add(newUser);
            _context.SaveChanges();

            return Ok(new { success = true });
        }
    }

   
}
