using Microsoft.AspNetCore.Mvc;
using KuaforSalonuProje.Models;

namespace KuaforSalonuProje.Controllers
{
    public class AccountController : Controller
    {
        private readonly KuaforContext _context;

        public AccountController(KuaforContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            var admin = _context.Admin.FirstOrDefault(a => a.KullaniciAdi == kullaniciAdi && a.Sifre == sifre);

            if (admin != null)
            {
                // Giriş başarılı, admin paneline yönlendir
                return RedirectToAction("Index", "Admin");
            }

            // Giriş başarısız, hata mesajı göster
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }
    }
}
