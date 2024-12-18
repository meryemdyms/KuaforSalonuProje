using Microsoft.AspNetCore.Mvc;

namespace KuaforSalonuProje.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
