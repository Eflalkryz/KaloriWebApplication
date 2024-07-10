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

            var userNutrients = _context.UserNutrients
                .Where(un => un.UserID == userId.Value)
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
