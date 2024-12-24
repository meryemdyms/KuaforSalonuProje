using Microsoft.AspNetCore.Mvc;
using KuaforSalonuProje.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KuaforSalonuProje.Controllers
{
    public class AccountController : Controller
    {
        private readonly KuaforContext _context;

        public AccountController(KuaforContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kullanıcı adı daha önce alınmış mı kontrol et
                    var mevcutKullanici = await _context.Kullanicilar
                        .FirstOrDefaultAsync(k => k.KullaniciAdi == kullanici.KullaniciAdi);

                    if (mevcutKullanici != null)
                    {
                        ViewBag.Error = "Bu kullanıcı adı zaten alınmış.";
                        return View();
                    }

                    // Kullanıcıyı veritabanına ekle
                    _context.Add(kullanici);
                    await _context.SaveChangesAsync();

                    // Kayıt başarılı olduktan sonra giriş sayfasına yönlendir
                    return RedirectToAction("Login", "Account");
                }

                catch (Exception ex)
                {
                    // Hata oluşursa hata mesajını göstermek için
                    ViewBag.Error = "Kayıt sırasında bir hata oluştu: " + ex.Message;
                    return View();
                }
            }

            // Model geçersizse
            ViewBag.Error = "Lütfen tüm alanları doğru şekilde doldurduğunuzdan emin olun.";
            return View();
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
                // Kullanıcı giriş başarılı, kullanıcı bilgilerini TempData'ya ekle
                TempData["UserName"] = $"{kullanici.Adi} {kullanici.Soyadi}"; // Kullanıcı adı ve soyadı birleşimi
                return RedirectToAction("Welcome", "Account"); // Welcome sayfasına yönlendir
            }

            // Giriş başarısız
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        // Hoşgeldiniz Sayfası
        [HttpGet]
        public IActionResult Welcome()
        {
            if (TempData["UserName"] == null)
            {
                return RedirectToAction("Login"); // Kullanıcı adı yoksa Login sayfasına yönlendir
            }

            ViewBag.UserName = TempData["UserName"]; // Kullanıcı adı ve soyadı bilgisi
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Kullanıcının oturumunu sonlandırmak için TempData'yı temizleyebilirsiniz
            TempData.Clear();

            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }

        
    }
}
