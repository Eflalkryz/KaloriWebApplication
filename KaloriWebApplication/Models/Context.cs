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
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CaloryNutrient> CaloryNutrients { get; set; }
        public DbSet<UserNutrient> UserNutrients { get; set; }
        public DbSet<TotalCalory> TotalCalories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserNutrient>()
                .HasOne(un => un.CaloryNutrient)
                .WithMany()
                .HasForeignKey(un => un.NutrientID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
