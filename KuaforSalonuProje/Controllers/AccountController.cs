using Microsoft.AspNetCore.Mvc;
using KuaforSalonuProje.Models;
using System.Linq;

namespace KuaforSalonuProje.Controllers
{
    public class AccountController : Controller
    {
        private readonly KuaforContext _context;

        public AccountController(KuaforContext context)
        {
            _context = context;
        }

        // Giriş Sayfası
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Giriş İşlemi
        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                ViewBag.Error = "Kullanıcı adı ve şifre gereklidir.";
                return View();
            }

            // Admin kontrolü
            var admin = _context.Admin.FirstOrDefault(a => a.KullaniciAdi == kullaniciAdi && a.Sifre == sifre);

            if (admin != null)
            {
                // Admin giriş başarılı, admin paneline yönlendir
                return RedirectToAction("Index", "Admin");
            }

            // Kullanıcı kontrolü
            var kullanici = _context.Kullanicilar.FirstOrDefault(u => u.KullaniciAdi == kullaniciAdi && u.Sifre == sifre);

            if (kullanici != null)
            {
                // Kullanıcı giriş başarılı, kullanıcı paneline yönlendir
                return RedirectToAction("Index", "Home");
            }

            // Giriş başarısız
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        // Kayıt Sayfası
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Aynı kullanıcı adıyla başka bir kullanıcı olup olmadığını kontrol et
            var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(u => u.KullaniciAdi == model.Email);

            if (mevcutKullanici != null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adıyla zaten bir hesap mevcut.");
                return View(model);
            }

            // Yeni kullanıcı ekle
            var yeniKullanici = new Kullanici
            {
                Adi = model.FirstName,
                Soyadi = model.LastName,
                KullaniciAdi = model.Email,
                Sifre = model.Password // NOT: Şifre hash'leme yapmanız önerilir
            };

            _context.Kullanicilar.Add(yeniKullanici);
            _context.SaveChanges(); // Kullanıcı veritabanına kaydediliyor

            // Başarılı kayıt sonrası mesaj göster ve giriş sayfasına yönlendir
            ViewBag.SuccessMessage = "İşleminiz başarıyla gerçekleştirildi.";
            return View("Register");
        }

    }
}
