using KaloriWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models.Concrete;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
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

        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Account Dashboard";
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
                    return RedirectToAction("Dashboard");
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
                // Check if the email is already registered
                var existingUserByEmail = _context.Users.FirstOrDefault(u => u.Eposta == model.Eposta);
                if (existingUserByEmail != null)
                {
                    ModelState.AddModelError("Eposta", "This email is already registered.");
                }

                // Check if the username is already taken
                var existingUserByUsername = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUserByUsername != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                
                model.RegisterDate = DateTime.UtcNow; 
                model.AdminRole = false; 

                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("CompleteProfile", new { userId = model.UserID });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CompleteProfile(int userId)
        {
            var profile = new CustomersProfile { UserID = userId };
            return View(profile);
        }

        [HttpPost]
        public IActionResult CompleteProfile(CustomersProfile model)
        {
            if (ModelState.IsValid)
            {
                _context.CustomersProfiles.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult TestCompleteProfile()
        {
            var profile = new CustomersProfile { UserID = 0 }; // Test için varsayılan bir UserID
            return RedirectToAction("CompleteProfile", new { userId = profile.UserID });
        }

        [HttpGet]
        public IActionResult NutrientSelect()
        {
            var categories = _context.CaloryNutrients
                .Select(c => c.FoodCategory)
                .Distinct()
                .ToList();

            ViewBag.Categories = categories;

            return View();
        }


        [HttpGet]
        public IActionResult GetFoodItems(string category)
        {
            var foodItems = _context.CaloryNutrients
                .Where(c => c.FoodCategory == category)
                .Select(c => c.FoodItem)
                .ToList();

            return Json(foodItems);
        }
    }



}

