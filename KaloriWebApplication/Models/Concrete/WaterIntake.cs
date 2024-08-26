namespace KaloriWebApplication.Models.Concrete
{
    public class WaterIntake
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal WaterAmount { get; set; }
        public DateTime DateConsumed { get; set; }
        public decimal Weight { get; set; }

    }
}