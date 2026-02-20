using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceLog.Models;

namespace ServiceLog.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ServiceRecord> ServiceRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Vehicle>()
           .HasOne(v => v.User)
           .WithMany(u => u.Vehicles)
           .HasForeignKey(v => v.UserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Vehicle>()
            .HasIndex(v => v.UserId);

        builder.Entity<ServiceRecord>()
            .HasOne(sr => sr.Vehicle)
            .WithMany(v => v.ServiceRecords)
            .HasForeignKey(sr => sr.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);
    }
}


