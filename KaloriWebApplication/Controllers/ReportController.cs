using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models;
using KaloriWebApplication.Models.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KaloriWebApplication.Controllers
{
    public class ReportController : Controller
    {
        private readonly Context _context;

        public ReportController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult DailyReport()
        {
            return View("~/Views/Account/DailyReport.cshtml");
        }

        [HttpGet]
        public IActionResult WeeklyReport()
        {
            return View("~/Views/Account/WeeklyReport.cshtml");
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

            return Json(new { success = true, data = result });
        }

        [HttpGet]
        public JsonResult GetUserNutrientsByWeek(DateTime startDate, DateTime endDate)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var userNutrients = _context.UserNutrients
                .Where(un => un.UserID == userId.Value && un.DateLogged.Date >= startDate.Date && un.DateLogged.Date <= endDate.Date)
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

            return Json(new { success = true, data = result });
        }

        [HttpGet]
        public IActionResult notification()
        {
           
            return View("~/Views/Account/notification.cshtml");
        }

       
    }
}
