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
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            List<notification> bildirim = _context.notifications
             .Where(un => un.UserID == userId.Value)
             .OrderByDescending(un => un.notificationDate)
              .Take(10)
             .ToList();


            return View(bildirim);
        }

        [HttpPost]
        public IActionResult notificatio(int a)
        {
           

            var existingUser = _context.notifications.FirstOrDefault(u => u.notificationID == a);
            if (existingUser == null)
            {
                return NotFound();
            }
            else
            {
                existingUser.isRead = 1;
                _context.SaveChanges();
            }




            return RedirectToAction("notification");
        }

        [HttpPost]
        public IActionResult notificati(int a)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            
                var notifications = _context.notifications.Where(n => n.UserID == userId).ToList();

                foreach (var notification in notifications)
                {
                    notification.isRead = a;
                }

                _context.SaveChanges();
            



            return RedirectToAction("notification");
        }
    }
}
