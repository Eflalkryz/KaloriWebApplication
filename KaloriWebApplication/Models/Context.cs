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

        public DbSet<notification> notifications { get; set; }

        public DbSet<WaterIntake> WaterIntakes { get; set; }

        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<UserCalorie> UserCalories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserNutrient>()
                .HasOne(un => un.CaloryNutrient)
                .WithMany()
                .HasForeignKey(un => un.NutrientID);

            modelBuilder.Entity<notification>(entity =>
            {
                entity.HasKey(e => e.notificationID); 
                entity.Property(e => e.notificationID).ValueGeneratedOnAdd(); 
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
