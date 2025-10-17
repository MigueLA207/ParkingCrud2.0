using CrudPark.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Data;

public class ParkingDbContext : DbContext
{
    public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
    {
    }
    
    public DbSet<Operator> Operators { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapea la entidad Operator a la tabla "operators" (en minúsculas)
        modelBuilder.Entity<Operator>(entity =>
        {
            entity.ToTable("operators");

            // También puedes mapear columnas si no coinciden
            // Por ejemplo, si tu columna se llama 'operator_id' en la BD:
            // entity.Property(e => e.OperatorId).HasColumnName("operator_id");
        });
    }
}