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

        


      

    }
}
