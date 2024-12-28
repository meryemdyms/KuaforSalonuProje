using Microsoft.AspNetCore.Mvc;
using KuaforSalonuProje.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace KuaforSalonuProje.Controllers
{
    public class AccountController : Controller
    {
        private readonly KuaforContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(KuaforContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
                HizmetId = hizmet.HizmetId
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

        [HttpPost]
        public async Task<IActionResult> ProcessPhoto(IFormFile photo, [FromServices] IConfiguration configuration)
        {
            if (photo == null || photo.Length == 0)
            {
                return Json(new { success = false, message = "Lütfen bir fotoğraf yükleyin." });
            }

            try
            {
                // OpenAI API anahtarını tanımlayın
                var apiKey = "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";

                if (string.IsNullOrEmpty(apiKey))
                {
                    return Json(new { success = false, message = "API anahtarı bulunamadı." });
                }

                // Fotoğrafı geçici olarak kaydet
                var filePath = Path.Combine(Path.GetTempPath(), photo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // OpenAI API'ye istek yap
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    // GPT modeline metinsel açıklama gönderme
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        model = "gpt-3.5-turbo", // Güncel model
                        messages = new[]
                        {
                    new { role = "system", content = "Bir kişinin fotoğrafına göre saç modeli önerileri üret." },
                    new { role = "user", content = "Örnek saç modeli önerilerini listele." }
                },
                        max_tokens = 200,
                        temperature = 0.7
                    }), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        return Json(new { success = false, message = $"API çağrısı başarısız: {errorResponse}" });
                    }

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    var suggestions = ((IEnumerable<dynamic>)result.choices)
                        .Select(choice => (string)choice.message.content)
                        .Where(text => !string.IsNullOrWhiteSpace(text))
                        .ToList();

                    return Json(new { success = true, suggestions });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }




    }
}
