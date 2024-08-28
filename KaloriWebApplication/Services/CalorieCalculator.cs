using Microsoft.EntityFrameworkCore;
using KaloriWebApplication.Models;
using System;
using KaloriWebApplication.Models.Concrete;

namespace KaloriWebApplication.Services
{
    public class CalorieCalculator
    {
        private readonly Context _context;

        public CalorieCalculator(Context context)
        {
            _context = context;
        }

        public decimal CalculateCalories(int ExerciseID, decimal UserKG, decimal TimePassedInMin)
        {
            var exercise = _context.ExerciseTypes.Find(ExerciseID);

            if (exercise == null)
            {
                throw new Exception("No Exercise Found !");
            }

            decimal CaloriesBurned = exercise.MET * (UserKG / 200) * TimePassedInMin;

            return CaloriesBurned;
        }

        public void SaveBurnedCalories(int userID, decimal caloriesBurned)
        {
            var userCalories = new UserCalorie
            {
                UserID = userID,
                Date = DateTime.Now,
                TotalCalories = caloriesBurned
            };

            _context.UserCalories.Add(userCalories);
            _context.SaveChanges();
        }

        /*internal object GetExerciseById(int exerciseID)
        {
            throw new NotImplementedException();
        }*/
    }
}
