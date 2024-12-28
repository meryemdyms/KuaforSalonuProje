using KuaforSalonuProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KuaforSalonuProje.Controllers
{
    public class AdminController : Controller
    {
        private readonly KuaforContext _context;

        public AdminController(KuaforContext context)
        {
            _context = context;
        }

        // Admin Paneli: Çalışan Yönetimi
        [HttpGet]
        public IActionResult Index()
        {
            // Çalışanların listesini veritabanından al
            var calisanlar = _context.Calisanlar.ToList();
            return View(calisanlar);
        }

        // Yeni Çalışan Ekleme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult YeniCalisanEkle(Calisan model)
        {
            if (!ModelState.IsValid)
            {
                // Hataları listeleyin ve debug yapın
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.Errors = errors;
                TempData["ErrorMessage"] = "Tüm alanlar doldurulmalıdır: " + string.Join(", ", errors);
                return RedirectToAction("Index");
            }

            // Veritabanına ekleme işlemi
            _context.Calisanlar.Add(model);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Çalışan başarıyla eklendi!";
            return RedirectToAction("Index");
        }


        // Çalışan Silme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CalisanSil(int id)
        {
            var calisan = _context.Calisanlar.FirstOrDefault(c => c.CalisanId == id);
            if (calisan == null)
            {
                TempData["ErrorMessage"] = "Çalışan bulunamadı.";
                return RedirectToAction("Index");
            }

            // Çalışanı sil
            _context.Calisanlar.Remove(calisan);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Çalışan başarıyla silindi.";
            return RedirectToAction("Index");
        }

        public IActionResult RandevuYönetimi()
        {
            
            return View();
        }

        public IActionResult RandevuYoneticisi()
        {
            var randevular = _context.Randevular.ToList();
            return View(randevular);
        }


    }
}
