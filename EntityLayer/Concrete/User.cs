using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class User
    {
        public int UserID { get; set; }
        [StringLength(50)]
        public string Username { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(50)]
        public string Eposta { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool AdminRole { get; set; }

        public CustomersProfile CustomersProfiles { get; set; }
    }
}
