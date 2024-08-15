using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models.Concrete;
using System.Linq;
using KaloriWebApplication.Models;

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
                    // Storing user ID in session
                    HttpContext.Session.SetInt32("UserID", user.UserID);

                    // Create and add notification to database
                    var notification = new notification
                    {
                        UserID = user.UserID,
                        notificationText = "Login Successful",
                        notificationDate = DateTime.Now,
                        isRead = 0
                    };

                    _context.notifications.Add(notification);
                    _context.SaveChanges();

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
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users.Find(userId);
            if (user != null)
            {
                ViewBag.UserName = user.Name;
            }
            else
            {
                ViewBag.UserName = "Guest";
            }

            ViewData["Title"] = "Account Dashboard";
            return View();
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
                var existingUserByEmail = _context.Users.FirstOrDefault(u => u.Eposta == model.Eposta);
                if (existingUserByEmail != null)
                {
                    ModelState.AddModelError("Eposta", "This email is already registered.");
                    return View(model);
                }

                var existingUserByUsername = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUserByUsername != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
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

        #region Profile Completing

        [HttpGet]
        public IActionResult CompleteProfile(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                return NotFound();
            }

            var profile = new User
            {
                UserID = userId
            };
            return View(profile);
        }

        [HttpPost]
        public IActionResult CompleteProfile(User model)
        {
            if (ModelState.IsValid)
            {
                // BMR hesaplama
                var bmr = CalculateBMR(model);

                // Günlük kalori ihtiyacını hesaplama
                var dailyCalories = CalculateDailyCalories(bmr, model.ActivityLevel);

                model.DailyCalories = (int)dailyCalories; // Günlük kalori ihtiyacı integer olmalı

                // Kullanıcının profili güncelle
                var existingUser = _context.Users.FirstOrDefault(u => u.UserID == model.UserID);
                if (existingUser != null)
                {
                    existingUser.Name = model.Name;
                    existingUser.Age = model.Age;
                    existingUser.Gender = model.Gender;
                    existingUser.Height = model.Height;
                    existingUser.Weight = model.Weight;
                    existingUser.ActivityLevel = model.ActivityLevel;
                    existingUser.Goal = model.Goal;
                    existingUser.DailyCalories = model.DailyCalories;

                    _context.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
            }
            return View(model);
        }

        private double CalculateBMR(User user)
        {
            // Basal Metabolic Rate (BMR) hesaplama
            // BMR hesaplama formülü: 
            // - Erkekler için: 10 * ağırlık(kg) + 6.25 * boy(cm) - 5 * yaş + 5
            // - Kadınlar için: 10 * ağırlık(kg) + 6.25 * boy(cm) - 5 * yaş - 161
            if (user.Gender == "Male")
            {
                return 10 * user.Weight.Value + 6.25 * user.Height.Value - 5 * user.Age.Value + 5;
            }
            else if (user.Gender == "Female")
            {
                return 10 * user.Weight.Value + 6.25 * user.Height.Value - 5 * user.Age.Value - 161;
            }
            else
            {
                // Eğer cinsiyet belirtilmemişse veya "Other" gibi bir değer varsa, varsayılan olarak erkek formülünü kullan
                return 10 * user.Weight.Value + 6.25 * user.Height.Value - 5 * user.Age.Value + 5;
            }
        }

        private double CalculateDailyCalories(double bmr, string activityLevel)
        {
            // Aktivite seviyesine göre kalori ihtiyacını hesaplama
            switch (activityLevel)
            {
                case "Sedentary":
                    return bmr * 1.2;
                case "Lightly Active":
                    return bmr * 1.375;
                case "Moderately Active":
                    return bmr * 1.55;
                case "Very Active":
                    return bmr * 1.725;
                case "Extra Active":
                    return bmr * 1.9;
                default:
                    return bmr; // Varsayılan olarak sadece BMR
            }
        }
        #endregion

        #region NutrientSelect

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
        #endregion

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult DailyWeeklyReports()
        {
            return View();
        }
        
    }


}

