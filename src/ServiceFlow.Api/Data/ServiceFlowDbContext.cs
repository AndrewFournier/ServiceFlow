using Microsoft.EntityFrameworkCore;
using ServiceFlow.Api.Domain;

namespace ServiceFlow.Api.Data;

public sealed class ServiceFlowDbContext(
    DbContextOptions<ServiceFlowDbContext> options)
    : DbContext(options)
{
    public DbSet<ServiceOffering> Services => Set<ServiceOffering>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceOffering>(service =>
        {
            service.HasKey(item => item.Id);

            service.Property(item => item.Name)
                .HasMaxLength(100)
                .IsRequired();

            service.Property(item => item.Description)
                .HasMaxLength(500)
                .IsRequired();

            service.Property(item => item.StartingPrice)
                .HasPrecision(10, 2);

            service.Property(item => item.IsActive)
                .HasDefaultValue(true);

            service.HasData(
                new ServiceOffering
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Lawn Care",
                    Description = "Mowing, edging, and general yard cleanup.",
                    StartingPrice = 75.00m,
                    EstimatedMinutes = 90,
                    IsActive = true
                },
                new ServiceOffering
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Pressure Washing",
                    Description = "Exterior cleaning for driveways, patios, and siding.",
                    StartingPrice = 150.00m,
                    EstimatedMinutes = 120,
                    IsActive = true
                },
                new ServiceOffering
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Gutter Cleaning",
                    Description = "Removal of leaves and debris from gutters and downspouts.",
                    StartingPrice = 125.00m,
                    EstimatedMinutes = 90,
                    IsActive = true
                });
        });
    }
}
