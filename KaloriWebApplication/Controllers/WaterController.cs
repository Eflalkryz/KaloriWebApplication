using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models;
using KaloriWebApplication.Models.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Security.Claims;

namespace KaloriWebApplication.Controllers
{
    public class WaterController: Controller
    {
        private readonly Context _context;

        public WaterController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult WaterIntake()
        {
            return View("~/Views/Account/WaterIntake.cshtml");
        }

        [HttpPost]
        public IActionResult SaveUserWaterIntake(decimal amountInLiters)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to perform this action.";
                return RedirectToAction("Login", "Account");
            }

            var today = DateTime.Today;
            var existingWaterIntake = _context.WaterIntakes
                .FirstOrDefault(w => w.UserId == userId && w.DateConsumed == today);

            if (existingWaterIntake != null)
            {
                existingWaterIntake.WaterAmount += amountInLiters;
                _context.WaterIntakes.Update(existingWaterIntake);
            }
            else
            {
                var newWaterIntake = new WaterIntake
                {
                    UserId = userId.Value,
                    WaterAmount = amountInLiters,
                    DateConsumed = today
                };
                _context.WaterIntakes.Add(newWaterIntake);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Water intake added successfully!";
            return RedirectToAction("WaterIntake");
        }

        [HttpGet]
        public JsonResult GetUserWaterIntakeByDate(DateTime date)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var waterIntake = _context.WaterIntakes
                .FirstOrDefault(w => w.UserId == userId && w.DateConsumed == date.Date);

            if (waterIntake != null)
            {
                return Json(new { success = true, WaterAmount = waterIntake.WaterAmount });
            }

            return Json(new { success = false, message = "No data found for the selected date" });
        }

        [HttpGet]
        public JsonResult GetWeeklyWaterIntake(DateTime? startDate, DateTime? endDate)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            startDate ??= DateTime.Today.AddDays(-6);
            endDate ??= DateTime.Today;

            if (startDate > endDate)
            {
                return Json(new { error = "Start date cannot be later than end date." });
            }

            var weeklyWaterIntake = new List<object>();

            for (DateTime date = startDate.Value; date <= endDate.Value; date = date.AddDays(1))
            {
                var intakeForDate = _context.WaterIntakes
                    .Where(w => w.UserId == userId && w.DateConsumed == date.Date)
                    .Select(w => w.WaterAmount)
                    .FirstOrDefault();

                weeklyWaterIntake.Add(new
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    WaterAmount = intakeForDate != 0 ? intakeForDate : 0
                });
            }

            return Json(weeklyWaterIntake);
        }
    }
}
