using GymManagement.Areas.Identity.Data;
using GymManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Member> Members { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<WalkIn> WalkIns{ get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Packages
        modelBuilder.Entity<Package>().HasData(
            new Package { Id = 1, PackageName = "Gym Only", PackagePrice = 5000, Gender = "Male" },
            new Package { Id = 2, PackageName = "Gym + Treadmill", PackagePrice = 7000, Gender = "Male" },
            new Package { Id = 3, PackageName = "All in One Combo", PackagePrice = 10000, Gender = "Male" },
            new Package { Id = 4, PackageName = "Gym Only Female", PackagePrice = 5500, Gender = "Female" }
        );

        // Seed Trainers
        modelBuilder.Entity<Trainer>().HasData(
            new Trainer { Id = 1, FullName = "Ali Raza", TrainerType = "Gym Trainer", ContactNo = "03001234567", Status = "Active" },
            new Trainer { Id = 2, FullName = "Sara Khan", TrainerType = "Personal Trainer", ContactNo = "03007654321", Status = "Active" }
        );

        // Seed Members
        modelBuilder.Entity<Member>().HasData(
            new Member
            {
                Id = 1,
                FullName = "Ahmed Ali",
                Gender = "Male",
                Contact = "03111234567",
                MemberStatus = "Active",
                DayTiming = "Morning",
                AccountOpenDate = DateTime.Now.AddMonths(-2),
                PackageId = 1,
                PackageExpiryDate = DateTime.Now.AddMonths(1),
                TrainerId = 1,
                AdmissionFee = 5000,
                MonthlyFee = 5000,
                Discount = 0,
                TotalAmount = 5000,
                Account = "Cash",
                MemberImage=""
            },
            new Member
            {
                Id = 2,
                FullName = "Ayesha Fatima",
                Gender = "Female",
                Contact = "03127654321",
                MemberStatus = "Inactive",
                DayTiming = "Evening",
                AccountOpenDate = DateTime.Now.AddMonths(-1),
                PackageId = 4,
                PackageExpiryDate = DateTime.Now.AddDays(15),
                TrainerId = 2,
                AdmissionFee = 5500,
                MonthlyFee = 5500,
                Discount = 500,
                TotalAmount = 10500,
                Account = "Credit Card",
                MemberImage = ""
            }
        );
    }
}
