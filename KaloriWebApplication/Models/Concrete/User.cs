using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaloriWebApplication.Models.Concrete
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [StringLength(50)]
        public string? Username { get; set; }
        [StringLength(50)]
        public string? Password { get; set; }
        [StringLength(50)]
        public string? Eposta { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool AdminRole { get; set; }
        public String? Name { get; set; }
        public int? Age { get; set; }

        [StringLength(50)]
        public String? Gender { get; set; }

        public int? Height { get; set; }
        public int? Weight { get; set; }
        [StringLength(50)]
        public String? ActivityLevel { get; set; }

        [StringLength(50)]
        public String? Goal { get; set; } //  Gain weight ? Lose weight ?
        public int? DailyCalories { get; set; }

        public ICollection<TotalCalory>? TotalCalories { get; set; }
    }
}
