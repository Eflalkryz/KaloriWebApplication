using System.ComponentModel.DataAnnotations;

namespace KaloriWebApplication.Models.Concrete
{
    public class TotalCalory
    {
        [Key]
        public int CaloryID { get; set; }
        public int? UserID { get; set; }
        public int? TotalCal { get; set; }
        public DateTime? CaloryDate { get; set; }
        public virtual User? Users { get; set; }
    }
}
