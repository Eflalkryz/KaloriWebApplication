using System.ComponentModel.DataAnnotations;

namespace KaloriWebApplication.Models.Concrete
{
    public class notification
    {
        [Key]
        public int notificationID { get; set; }
       
        public string notificationText { get; set; }
        
        public DateTime notificationDate { get; set; }
        
        public int UserID { get; set; }

        public string? isRead {  get; set; }        
        
    }
}
