using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaloriWebApplication.Models.Concrete;
using KaloriWebApplication.Models;

namespace KaloriWebApplication.Models

{
    public class Context:DbContext
    {

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }
        public DbSet<Calory> Calories { get; set; }
        public DbSet<CustomersProfile> CustomersProfiles { get; set; } //S..ssss
        public DbSet<CaloryNutrient> CaloryNutrients { get; set; } 


    }
}
