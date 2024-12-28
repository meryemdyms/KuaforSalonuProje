using Microsoft.AspNetCore.Mvc;
using KuaforSalonuProje.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Model doğrulama başarısızsa formu tekrar göster
            }

            // Kullanıcı adı kontrolü
            var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(u => u.KullaniciAdi == model.KullaniciAdi);
            if (mevcutKullanici != null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı zaten kullanılıyor.");
                return View(model);
            }

            // Yeni kullanıcı oluştur
            var yeniKullanici = new Kullanici
            {
                Adi = model.FirstName,
                Soyadi = model.LastName,
                KullaniciAdi = model.KullaniciAdi,
                Sifre = model.Password
            };

            _context.Kullanicilar.Add(yeniKullanici);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Kayıt işlemi başarıyla tamamlandı.";
            return RedirectToAction("Login");
        }



        [HttpPost]
        public async Task<IActionResult> Login(string kullaniciAdi, string sifre)
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
                // Kullanıcı giriş başarılı, oturum bilgilerini ayarla
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
            new Claim("UserId", kullanici.KullaniciId.ToString()), // Kullanıcı ID'sini ekle
            new Claim(ClaimTypes.Role, "Kullanici") // Kullanıcı rolü
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Oturum aç
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Welcome", "Account");
            }

            // Giriş başarısız
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }
        
        [HttpGet]
        public IActionResult Welcome()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("Kullanıcı kimlik bilgisi bulunamadı.");
            }

            var kullaniciId = int.Parse(userIdClaim.Value);

            // Kullanıcıya ait randevular
            var randevular = _context.Randevular
                .Where(r => r.KullaniciId == kullaniciId)
                .Select(r => new
                {
                    r.IslemAdi,
                    r.ucret,
                    Tarih = r.RandevuTarihi.ToShortDateString(),
                    Saat = r.RandevuTarihi.ToShortTimeString(),
                    Calisan = r.Calisan.CalisanAdi + " " + r.Calisan.CalisanSoyadi,
                    r.Durum // Durum Bilgisi Dahil Edildi
                })
                .ToList<dynamic>();

            // Hizmetler
            var hizmetler = _context.Hizmetler.ToList();
            var calisanlar = _context.Calisanlar.ToList();

            foreach (var hizmet in hizmetler)
            {
                var hizmetVeren = calisanlar.FirstOrDefault(c => c.UzmanlikAlani == hizmet.HizmetAdi);
                hizmet.HizmetVeren = hizmetVeren != null
                    ? hizmetVeren.CalisanAdi + " " + hizmetVeren.CalisanSoyadi
                    : "Uygun çalışan bulunamadı";
            }

            ViewBag.Randevular = randevular;
            ViewBag.Hizmetler = hizmetler;
            ViewBag.UserName = User.Identity.Name;

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
        [HttpPost]
        public IActionResult RandevuOlustur(int HizmetId, DateTime Tarih, TimeSpan Saat)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Kullanıcı oturum açmamış." });
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Kullanıcı kimlik bilgisi bulunamadı." });
            }

            var kullaniciId = int.Parse(userIdClaim.Value);

            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.HizmetId == HizmetId);
            if (hizmet == null)
            {
                return Json(new { success = false, message = "Seçilen hizmet bulunamadı." });
            }

            var calisan = _context.Calisanlar.FirstOrDefault(c => c.UzmanlikAlani == hizmet.HizmetAdi);
            if (calisan == null)
            {
                return Json(new { success = false, message = "Bu hizmet için uygun çalışan bulunamadı." });
            }

            var randevuTarihiVeSaati = Tarih.Date.Add(Saat);

            // Aynı çalışanın aynı tarih ve saatte herhangi bir hizmete randevusu olup olmadığını kontrol et
            var mevcutRandevu = _context.Randevular
                .Where(r => r.CalisanId == calisan.CalisanId && r.RandevuTarihi == randevuTarihiVeSaati)
                .FirstOrDefault();

            if (mevcutRandevu != null)
            {
                return Json(new
                {
                    success = false,
                    message = $"Bu saatlerde {calisan.CalisanAdi} {calisan.CalisanSoyadi} başka bir randevuya sahiptir. Lütfen başka bir saat seçiniz."
                });
            }

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciId == kullaniciId);
            if (kullanici == null)
            {
                return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı." });
            }

            var randevu = new Randevu
            {
                RandevuTarihi = randevuTarihiVeSaati,
                RandevuSaati = randevuTarihiVeSaati,
                IslemAdi = hizmet.HizmetAdi,
                ucret = (double)hizmet.Ucret,
                CalisanId = calisan.CalisanId,
                KullaniciId = kullanici.KullaniciId,
                HizmetId = hizmet.HizmetId,
               
            };

            try
            {
                _context.Randevular.Add(randevu);
                _context.SaveChanges();
                return Json(new { success = true, message = "Randevunuz başarıyla oluşturuldu!" });
            }
            catch (DbUpdateException ex)
            {
                return Json(new { success = false, message = $"Randevu kaydedilirken bir hata oluştu: {ex.InnerException?.Message}" });
            }
        }






        public IActionResult KullaniciRandevulari()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Kullanıcı oturum açmamış.");
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("Kullanıcı kimlik bilgisi bulunamadı.");
            }

            var kullaniciId = int.Parse(userIdClaim.Value);

            var randevular = _context.Randevular
                .Where(r => r.KullaniciId == kullaniciId)
                .Select(r => new
                {
                    r.IslemAdi,
                    r.ucret,
                    Tarih = r.RandevuTarihi.ToShortDateString(),
                    Saat = r.RandevuTarihi.ToShortTimeString(),
                    Calisan = r.Calisan.CalisanAdi + " " + r.Calisan.CalisanSoyadi
                })
                .ToList();

            return View(randevular);
        }


    }

}
