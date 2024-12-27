﻿using Microsoft.AspNetCore.Mvc;
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

            // Hizmetler ve Calisanları al
            var hizmetler = _context.Hizmetler.ToList(); // Hizmetler tablosundaki tüm veriler
            var calisanlar = _context.Calisanlar.ToList(); // Calisanlar tablosundaki tüm veriler

            // Her hizmeti ve ilgili çalışanı eşleştir
            foreach (var hizmet in hizmetler)
            {
                // Hizmet adı ile çalışan uzmanlık alanını eşleştir
                var hizmetVeren = calisanlar.FirstOrDefault(c => c.UzmanlikAlani == hizmet.HizmetAdi);
                if (hizmetVeren != null)
                {
                    hizmet.HizmetVeren = hizmetVeren.CalisanAdi + " " + hizmetVeren.CalisanSoyadi; // Hizmet verenin ismini ekle
                }
            }

            // Hizmetleri View'a gönder
            ViewBag.Hizmetler = hizmetler;

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
