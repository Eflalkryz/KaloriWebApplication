using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaloriWebApplication.Models.Concrete
{
    public class Nutrient
    {
        [Key]
        public int NutrientID { get; set; }

        [StringLength(50)]
        public String? NutrientName { get; set; }
        public DateTime NutrientEntryDate { get; set; }
        public int NutrientAmount { get; set; }
        public int NutrientCalory { get; set; }

        public int UserID { get; set; }  // Foreign key

        public virtual User? User { get; set; }  // Navigation property

    }
}
