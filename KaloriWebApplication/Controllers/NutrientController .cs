using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models;
using KaloriWebApplication.Models.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Security.Claims;

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
        public JsonResult GetFoodItems(string category, string searchQuery)
        {
            var foodItemsQuery = _context.CaloryNutrients.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                foodItemsQuery = foodItemsQuery.Where(n => n.FoodCategory == category);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                foodItemsQuery = foodItemsQuery.Where(n => n.FoodItem.Contains(searchQuery));
            }

            var foodItems = foodItemsQuery.Select(n => new { id = n.NutrientID, name = n.FoodItem }).ToList();

            return Json(foodItems);
        }


        [HttpPost]
        public IActionResult SaveUserNutrient(int nutrientId, decimal totalQuantity)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to perform this action.";
                return RedirectToAction("Login", "Account");
            }

            var nutrient = _context.CaloryNutrients.Find(nutrientId);
            if (nutrient == null)
            {
                TempData["ErrorMessage"] = "Invalid nutrient selected.";
                return RedirectToAction("NutrientSelect");
            }

            var calsPer100Grams = nutrient.Cals_per100grams?.Replace(" cal", "") ?? "0";
            if (!int.TryParse(calsPer100Grams, out int calsPer100GramsValue))
            {
                TempData["ErrorMessage"] = "Invalid calorie value.";
                return RedirectToAction("NutrientSelect");
            }

            int totalCalories = (int)(calsPer100GramsValue * (totalQuantity / 100));
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
                    UserID = userId.Value,
                    TotalCal = totalCalories,
                    CaloryDate = today
                };
                _context.TotalCalories.Add(newCalory);
            }

            var userNutrient = new UserNutrient
            {
                UserID = userId.Value,
                NutrientID = nutrientId,
                Quantity = totalQuantity,
                DateLogged = DateTime.Now
            };

            _context.UserNutrients.Add(userNutrient);

            var user = _context.Users.Find(userId);
            if (user != null && existingCalory != null)
            {
                int dailyCalorieLimit = user.DailyCalories ?? 0;

                if (existingCalory.TotalCal > dailyCalorieLimit)
                {
                    var notification = new notification
                    {
                        UserID = userId.Value,
                        notificationText = "You've exceeded your daily calorie limit.",
                        notificationDate = DateTime.Now,
                        isRead = 0
                    };
                    _context.notifications.Add(notification);
                    TempData["SuccessMessage"] = "Nutrient added successfully, but you've exceeded your daily calorie limit.";
                }
                else if (existingCalory.TotalCal > dailyCalorieLimit - 500)
                {
                    var notification = new notification
                    {
                        UserID = userId.Value,
                        notificationText = "You're close to exceeding your daily calorie limit.",
                        notificationDate = DateTime.Now,
                        isRead =0 
                    };
                    _context.notifications.Add(notification);
                    TempData["SuccessMessage"] = "Nutrient added successfully, but you're close to exceeding your daily calorie limit.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Nutrient added successfully!";
                }
            }

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
                userNutrientId = un.UserNutrientID,
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

            TempData["daily"] = totalCalories;
           
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
                userNutrientId = un.UserNutrientID,
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
        public IActionResult DailyCalories()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            int dailyCalories = user.DailyCalories ?? 0;

            return Json(new { success = true, dailyCalories });
        }

        [HttpPost]
        public IActionResult NotificationSave()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

                var not = new notification
                {
                    UserID = userId.Value, 
                    notificationText = "Aştınız",
                    notificationDate = DateTime.Now
                };

               
                 _context.notifications.Add(not);
                 _context.SaveChanges();
            

            return View();
        }


        [HttpPost]
        public JsonResult DeleteUserNutrient(int userNutrientId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var userNutrient = _context.UserNutrients.Include(un => un.CaloryNutrient)
                .FirstOrDefault(un => un.UserNutrientID == userNutrientId && un.UserID == userId);

            if (userNutrient == null)
            {
                return Json(new { success = false, message = "Nutrient not found" });
            }

            var calsPer100Grams = userNutrient.CaloryNutrient.Cals_per100grams?.Replace(" cal", "") ?? "0";
            if (!int.TryParse(calsPer100Grams, out int calsPer100GramsValue))
            {
                return Json(new { success = false, message = "Invalid calorie value" });
            }

            int totalCalories = (int)(calsPer100GramsValue * (userNutrient.Quantity / 100));

            var today = DateTime.Today;
            var existingCalory = _context.TotalCalories.FirstOrDefault(c => c.UserID == userId && c.CaloryDate == today);

            if (existingCalory != null)
            {
                existingCalory.TotalCal -= totalCalories;
                _context.TotalCalories.Update(existingCalory);
            }

            _context.UserNutrients.Remove(userNutrient);
            _context.SaveChanges();

            return Json(new { success = true, message = "Nutrient deleted" });
        }


        [HttpGet]
        public JsonResult GetWeeklyCalories(DateTime? startDate, DateTime? endDate)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            startDate ??= DateTime.Today.AddDays(-6);
            endDate ??= DateTime.Today;

            // Eğer tarih aralığı ters ise, düzeltilir.
            if (startDate > endDate)
            {
                return Json(new { error = "Başlangıç tarihi bitiş tarihinden büyük olamaz." });
            }

            var weeklyCalories = new List<object>();

            for (DateTime date = startDate.Value; date <= endDate.Value; date = date.AddDays(1))
            {
                var caloriesForDate = _context.TotalCalories
                    .Where(tc => tc.UserID == userId && tc.CaloryDate == date.Date)
                    .Select(tc => tc.TotalCal)
                    .FirstOrDefault();

                weeklyCalories.Add(new
                {
                    Date = date.ToString("yyyy-MM-dd"), // We're returning the date as a string.
                    TotalCalories = caloriesForDate != 0 ? caloriesForDate : 0
                });
            }

            return Json(weeklyCalories);
        }







    }
}