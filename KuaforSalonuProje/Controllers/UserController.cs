using KuaforSalonuProje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KuaforSalonuProje.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly KuaforSalonuContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(KuaforSalonuContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ManageAppointments()
        {
            var user = await _userManager.GetUserAsync(User);
            var appointments = _context.Randevular.Where(r => r.KullaniciId.ToString() == user.Id).ToList();
            return View(appointments);
        }

        public IActionResult UserPanel()
        {
            return View();
        }
    }
}
