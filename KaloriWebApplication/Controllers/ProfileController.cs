using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models.Concrete;
using System.Linq;
using KaloriWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace KaloriWebApplication.Controllers
{
    public class ProfileController : Controller
    {
        private readonly Context _context;

        public ProfileController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ProfileEditing()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // userId ile kullanıcı profili verilerini çekme işlemi
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult ProfileEditing(User model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var existingUser = _context.Users.FirstOrDefault(u => u.UserID == userId);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Username = model.Username;
                existingUser.Password = model.Password;
                existingUser.Eposta = model.Eposta;
                existingUser.Name = model.Name;
                existingUser.Age = model.Age;
                existingUser.Gender = model.Gender;
                existingUser.Height = model.Height;
                existingUser.Weight = model.Weight;
                existingUser.ActivityLevel = model.ActivityLevel;
                existingUser.Goal = model.Goal;
                existingUser.DailyCalories = (int) CalculateDailyCalories(model);

                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Account");
            }

            return View(model);
        }

        private double CalculateDailyCalories(User user)
        {
            // Aktivite seviyesine göre günlük kalori ihtiyacını hesaplama
            double bmr = CalculateBMR(user);
            switch (user.ActivityLevel)
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
                    return bmr;
            }
        }

        private double CalculateBMR(User user)
        {
            // Basal Metabolic Rate (BMR) hesaplama
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
                // Default olarak erkek formülünü kullan
                return 10 * user.Weight.Value + 6.25 * user.Height.Value - 5 * user.Age.Value + 5;
            }
        }
    }
    }

