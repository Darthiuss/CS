using Microsoft.EntityFrameworkCore;
using SensorMonitor.Core.Entities;

namespace SensorMonitor.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<FactoryEntity> Factories => Set<FactoryEntity>();
    public DbSet<SensorEntity> Sensors => Set<SensorEntity>();
    public DbSet<ReadingEntity> Readings => Set<ReadingEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FactoryEntity>(e =>
        {
            e.ToTable("Factories");
            e.HasKey(x => x.Id);
            e.Property(x => x.FactoryId).IsRequired().HasMaxLength(50);
            e.Property(x => x.FactoryName).IsRequired().HasMaxLength(200);
            e.HasIndex(x => x.FactoryId).IsUnique();
        });

        modelBuilder.Entity<SensorEntity>(e =>
        {
            e.ToTable("Sensors");
            e.HasKey(x => x.Id);
            e.Property(x => x.SensorId).IsRequired().HasMaxLength(50);
            e.Property(x => x.Type).IsRequired().HasMaxLength(50);
            e.HasIndex(x => new { x.FactoryEntityId, x.SensorId }).IsUnique();
            e.HasIndex(x => x.Type);

            e.HasOne(x => x.Factory)
                .WithMany(f => f.Sensors)
                .HasForeignKey(x => x.FactoryEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReadingEntity>(e =>
        {
            e.ToTable("Readings");
            e.HasKey(x => x.Id);
            e.Property(x => x.Status).IsRequired().HasMaxLength(20);
            e.HasIndex(x => new { x.SensorEntityId, x.Timestamp });
            e.HasIndex(x => x.Status);

            e.HasOne(x => x.Sensor)
                .WithMany(s => s.Readings)
                .HasForeignKey(x => x.SensorEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
