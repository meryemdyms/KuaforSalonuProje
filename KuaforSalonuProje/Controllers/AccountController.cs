using KuaforSalonuProje.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KuaforSalonuProje.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Kullanici> _userManager;
        private readonly SignInManager<Kullanici> _signInManager;

        public AccountController(UserManager<Kullanici> userManager, SignInManager<Kullanici> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Kullanici model)
        {
            if (ModelState.IsValid)
            {
                var user = new Kullanici
                {
                    Adi = model.Email,
                    Email = model.Email,
                    Rol = Roller.Kullanici // Default olarak 'Kullanici' rolü
                };
                var result = await _userManager.CreateAsync(user, model.Sifre);

                if (result.Succeeded)
                {
                    // Admin kontrolü eklemek
                    if (user.Rol == Roller.Admin)
                    {
                        await _userManager.AddToRoleAsync(user, Roller.Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, Roller.Kullanici);
                    }

                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }



    }
}

