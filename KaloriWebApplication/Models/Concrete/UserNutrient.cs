namespace KaloriWebApplication.Models.Concrete
{
    public class UserNutrient
    {
        public int UserNutrientID { get; set; }
        public int UserID { get; set; }
        public int NutrientID { get; set; }
        public decimal Quantity { get; set; }
        public DateTime DateLogged { get; set; }

        public CaloryNutrient CaloryNutrient { get; set; } // Navigation property
    }
}
