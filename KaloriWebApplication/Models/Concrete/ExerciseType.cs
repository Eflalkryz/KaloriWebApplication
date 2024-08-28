namespace KaloriWebApplication.Models.Concrete
{
    public class ExerciseType
    {
        public int ID { get; set; }

        public string ExerciseName { get; set; }

        public decimal MET { get; set; }

    }

    public class UserCalorie
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalCalories { get; set; }

    }

    public class ExerciseInputModel
    {
        public List<ExerciseType> Exercises { get; set; }
    }
}
