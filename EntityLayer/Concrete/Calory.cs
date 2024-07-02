using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Calory
    {
        public int CaloryID { get; set; }
        public int CustomerID { get; set; }
        public int TotalCalory { get; set; }
        public DateTime CaloryEntryDate { get; set; }
        public virtual CustomersProfile CustomersProfiles { get; set; }
        public virtual Nutrient Nutrients { get; set; }


    }
}
