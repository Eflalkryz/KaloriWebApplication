using KaloriWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models.Concrete;

namespace KaloriWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context;

        public AccountController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Yeni kullanıcıyı ekleyerek veritabanına kaydetme
                model.RegisterDate = DateTime.UtcNow; // Otomatik olarak kayıt tarihini ayarla
                model.AdminRole = false; // Yeni kullanıcılar için admin rolü varsayılan olarak false

                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}
