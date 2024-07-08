using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace KaloriWebApplication.Models.Concrete

{
    public class CustomersProfile
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        public int UserID { get; set; }
        [StringLength(50)]

        public String? Name { get; set; }
        public int Age { get; set; }

        [StringLength(50)]
        public String? Gender { get; set; }

        public int Height { get; set; }
        public int Weight { get; set; }
        [StringLength(50)]
        public String? ActivityLevel { get; set; }

        [StringLength(50)]
        public String? Goal { get; set; } //Kilo Alma mı ? Verme mi ?
        public int DailyCaloryLimit { get; set; }
        [Required]
        public virtual User Users { get; set; }
        public ICollection<Calory>? Calories { get; set; }


    }
}
