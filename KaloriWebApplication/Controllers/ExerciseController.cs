using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models;
using KaloriWebApplication.Models.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Security.Claims;
using KaloriWebApplication.Services;


namespace KaloriWebApplication.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly CalorieCalculator _calorieCalculator;
        private readonly Context _context;

        public ExerciseController(CalorieCalculator calorieCalculator, Context context)
        {
            _calorieCalculator = calorieCalculator;
            _context = context;
        }

        [HttpPost]
        public IActionResult Calculate(int ExerciseID, decimal UserKG, int TimePassedInMin, int UserID)
        {
            if (UserKG < 0 || TimePassedInMin < 0)
            {
                ModelState.AddModelError("", "Please Enter Valid Weight and Time Value");
                return View("Error");
            }

            /* var exercise = _calorieCalculator.GetExerciseById(ExerciseID);
             if (exercise == null)
             {
                 ModelState.AddModelError("", "Please Choose a Valid Exercise Type");
                 return View("Error");
             }*/

            var caloriesBurned = _calorieCalculator.CalculateCalories(ExerciseID, UserKG, TimePassedInMin);
            _calorieCalculator.SaveBurnedCalories(UserID, caloriesBurned);

            return View("Result", caloriesBurned);
        }

        public IActionResult DisplayExerciseForm()
        {
            var exercises = _context.ExerciseTypes.ToList();
            var model = new ExerciseInputModel
            {
                Exercises = exercises
            };

            return View("ExerciseEntryForm", model);

        }
    }
}

