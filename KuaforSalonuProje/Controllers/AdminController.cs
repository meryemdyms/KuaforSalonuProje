using Microsoft.AspNetCore.Mvc;

namespace KuaforSalonuProje.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
