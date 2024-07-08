using System.ComponentModel.DataAnnotations;

namespace KaloriWebApplication.Models.Concrete
{
    public class CaloryNutrient
    {
        [Key]
        public int NutrientID { get; set; }
        [StringLength(50)]
        public string? FoodItem { get; set; }
        [StringLength(50)]
        public string? FoodCategory { get; set; }
        [StringLength(50)]
        public string? per100grams { get; set; }
        [StringLength(50)]
        public string? Cals_per100grams { get; set; }
        [StringLength(50)]
        public string? KJ_per100grams { get; set;}
    }
}
