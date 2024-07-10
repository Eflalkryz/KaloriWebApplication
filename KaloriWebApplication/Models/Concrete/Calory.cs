using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaloriWebApplication.Models.Concrete
{
    public class Calory
    {
        [Key]
        public int CaloryID { get; set; }
        public int? UserID { get; set; }
        public int? TotalCalory { get; set; }
        public DateTime? CaloryEntryDate { get; set; }
        public virtual User? Users { get; set; }


    }
}
