using CrudPark.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Data;

public class ParkingDbContext : DbContext
{
    public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
    {
    }
    
    public DbSet<Operator> Operators { get; set; }
    public DbSet<Customer> Customers { get; set; } 
    public DbSet<Membership> Memberships { get; set; }
    
    public DbSet<Rate> Rates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Mapeo para Operator ---
        modelBuilder.Entity<Operator>(entity =>
        {
            entity.ToTable("operators");
            entity.HasKey(e => e.OperatorId);
            entity.Property(e => e.OperatorId).HasColumnName("operator_id");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        // --- Mapeo para Customer ---
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");
            entity.HasKey(e => e.CustomerId);
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.Email).HasColumnName("email");
        });

        // --- Mapeo para Membership ---
        modelBuilder.Entity<Membership>(entity =>
        {
            entity.ToTable("memberships");
            entity.HasKey(e => e.MembershipId);
            entity.Property(e => e.MembershipId).HasColumnName("membership_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.LicensePlate).HasColumnName("license_plate");
            entity.Property(e => e.VehicleType).HasColumnName("vehicle_type");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");

            // Define la relación entre las tablas
            entity.HasOne(m => m.Customer)
                .WithMany(c => c.Memberships)
                .HasForeignKey(m => m.CustomerId);
        });
        
        modelBuilder.Entity<Rate>(entity =>
        {
            entity.ToTable("rates");
            entity.HasKey(e => e.RateId);
            entity.Property(e => e.RateId).HasColumnName("rate_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.VehicleType).HasColumnName("vehicle_type");
            entity.Property(e => e.HourlyRate).HasColumnName("hourly_rate");
            entity.Property(e => e.FractionRate).HasColumnName("fraction_rate");
            entity.Property(e => e.DailyCap).HasColumnName("daily_cap");
            entity.Property(e => e.GracePeriodMinutes).HasColumnName("grace_period_minutes");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
        });
    }
}