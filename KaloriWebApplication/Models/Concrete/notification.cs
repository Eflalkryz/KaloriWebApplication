using System.ComponentModel.DataAnnotations;

namespace KaloriWebApplication.Models.Concrete
{
    public class notification
    {
        [Key]
        public int notificationID { get; set; }
        public DateTime notificationEntryDate { get; set; }
        public string notificationTitle { get; set;}
        
        public int UserID { get; set; } 
    }
}



