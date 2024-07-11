using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models;
using KaloriWebApplication.Models.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace KaloriWebApplication.Controllers
{
    public class NutrientController : Controller
    {
        private readonly Context _context;

        public NutrientController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult NutrientSelect()
        {
            ViewBag.Categories = _context.CaloryNutrients.Select(n => n.FoodCategory).Distinct().ToList();
            return View("~/Views/Account/NutrientSelect.cshtml");
        }

        [HttpGet]
        public JsonResult GetFoodItems(string category)
        {
            var foodItems = _context.CaloryNutrients
                .Where(n => n.FoodCategory == category)
                .Select(n => new { id = n.NutrientID, name = n.FoodItem })
                .ToList();

            return Json(foodItems);
        }

        [HttpPost]
        public IActionResult SaveUserNutrient(int nutrientId, decimal quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var nutrient = _context.CaloryNutrients.Find(nutrientId);
            if (nutrient == null)
            {
                return BadRequest("Invalid nutrient selected.");
            }

            var calsPer100Grams = nutrient.Cals_per100grams?.Replace(" cal", "") ?? "0";
            if (!int.TryParse(calsPer100Grams, out int calsPer100GramsValue))
            {
                return BadRequest("Invalid calorie value.");
            }

            int totalCalories = (int)(calsPer100GramsValue * (quantity / 100));

            var today = DateTime.Today;

            var existingCalory = _context.TotalCalories
                .FirstOrDefault(c => c.UserID == userId && c.CaloryDate == today);

            if (existingCalory != null)
            {
                existingCalory.TotalCal += totalCalories;
                _context.TotalCalories.Update(existingCalory);
            }
            else
            {
                var newCalory = new TotalCalory
                {
                    UserID = userId,
                    TotalCal = totalCalories,
                    CaloryDate = today
                };
                _context.TotalCalories.Add(newCalory);
            }

            var userNutrient = new UserNutrient
            {
                UserID = userId.Value,
                NutrientID = nutrientId,
                Quantity = quantity,
                DateLogged = DateTime.Now
            };

            _context.UserNutrients.Add(userNutrient);
            _context.SaveChanges();

            return RedirectToAction("NutrientSelect");
        }

        [HttpGet]
        public JsonResult GetUserNutrients()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var today = DateTime.Today;
            var userNutrients = _context.UserNutrients
                .Where(un => un.UserID == userId.Value && un.DateLogged.Date == today)
                .Include(un => un.CaloryNutrient)
                .OrderByDescending(un => un.DateLogged)
                .ToList();

            var result = userNutrients.Select(un => new
            {
                dateLogged = un.DateLogged,
                caloryNutrient = new
                {
                    foodItem = un.CaloryNutrient.FoodItem,
                    cals_per100grams = un.CaloryNutrient.Cals_per100grams
                },
                quantity = un.Quantity
            });

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetTotalCaloriesByDate(DateTime date)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var totalCalories = _context.TotalCalories
                .Where(tc => tc.UserID == userId && tc.CaloryDate == date.Date)
                .FirstOrDefault();

            if (totalCalories != null)
            {
                return Json(new { success = true, totalCalories = totalCalories.TotalCal });
            }

            return Json(new { success = false, message = "No data found for the selected date" });
        }

        [HttpGet]
        public JsonResult GetUserNutrientsByDate(DateTime date)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var userNutrients = _context.UserNutrients
                .Where(un => un.UserID == userId.Value && un.DateLogged.Date == date.Date)
                .Include(un => un.CaloryNutrient)
                .OrderByDescending(un => un.DateLogged)
                .ToList();

            var result = userNutrients.Select(un => new
            {
                dateLogged = un.DateLogged,
                caloryNutrient = new
                {
                    foodItem = un.CaloryNutrient.FoodItem,
                    cals_per100grams = un.CaloryNutrient.Cals_per100grams
                },
                quantity = un.Quantity
            });

            return Json(result);
        }
    }
}
