using KuaforSalonuProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KuaforSalonuProje.Controllers
{
    public class AdminController : Controller
    {
        private readonly KuaforContext _context;

        public AdminController(KuaforContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCalisan([FromBody] Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Calisanlar.Add(calisan);
                    _context.SaveChanges();
                    return Ok(new { message = "Çalışan başarıyla eklendi." });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Bir hata oluştu.", error = ex.Message });
                }
            }
            return BadRequest(new
            {
                message = "Hatalı veri gönderildi.",
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }
    }
}
