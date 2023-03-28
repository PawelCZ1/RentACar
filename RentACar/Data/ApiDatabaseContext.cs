using Microsoft.EntityFrameworkCore;
using RentACar.Models.Db;

namespace RentACar.Data;

public class ApiDatabaseContext : DbContext
{
    public ApiDatabaseContext(DbContextOptions options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }
    
    public DbSet<CustomerEntity> CustomerEntities { get; set; }
    public DbSet<CarEntity> CarEntities { get; set; }
    public DbSet<ReservationEntity> ReservationEntities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>()
            .HasMany(s => s.ReservationEntities)
            .WithOne(e => e.CustomerEntity);
        
        modelBuilder.Entity<CarEntity>()
            .HasMany(s => s.ReservationEntities)
            .WithOne(e => e.CarEntity);

        modelBuilder.Entity<CustomerEntity>().HasData(
            new CustomerEntity
            {
                Id = 1,
                Name = "Jan",
                Surname = "Kowalski",
                Nationality = "Poland",
                Balance = 3500,
                BirthDate = new DateTime(1995, 8, 3),
                RegistrationDate = DateTime.UtcNow
            },
            new CustomerEntity
            {
                Id = 2,
                Name = "Adam",
                Surname = "Nowak",
                Nationality = "Poland",
                Balance = 750,
                BirthDate = new DateTime(2003, 6, 14),
                RegistrationDate = DateTime.UtcNow
            },
            new CustomerEntity
            {
                Id = 3,
                Name = "Giorno",
                Surname = "Giovanna",
                Nationality = "Italy",
                Balance = 6500,
                BirthDate = new DateTime(1985, 4, 16),
                RegistrationDate = DateTime.UtcNow
            }
        );
    }
}