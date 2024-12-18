using KuaforSalonuProje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KuaforSalonuProje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly KuaforSalonuContext _context;

        public AdminController(KuaforSalonuContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Admin, çalışanları yönetebilecek
        public IActionResult ManageEmployees()
        {
            var employees = _context.Calisanlar.ToList();
            return View(employees);
        }

        // Admin, salonları yönetebilecek
        public IActionResult ManageSalons()
        {
            var salons = _context.Salonlar.ToList();
            return View(salons);
        }

        // Admin, randevuları onaylayabilecek
        public IActionResult ManageAppointments()
        {
            var appointments = _context.Randevular.ToList();
            return View(appointments);
        }

        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
