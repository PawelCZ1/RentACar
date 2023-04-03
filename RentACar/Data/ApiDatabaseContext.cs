using Microsoft.EntityFrameworkCore;
using RentACar.Models.Db;
using RentACar.Models.Enum;
using RentACar.Utils;

namespace RentACar.Data;

public class APIDatabaseContext : DbContext
{
    public APIDatabaseContext(DbContextOptions options) : base(options)
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

        modelBuilder.Entity<CarEntity>().Property(e => e.FuelType)
            .HasConversion(
                v => v.ToString(),
                v => CommonMethods.StringToEnum<FuelType>(v)
            );
        
        modelBuilder.Entity<CarEntity>().Property(e => e.GearboxType)
            .HasConversion(
                v => v.ToString(),
                v => CommonMethods.StringToEnum<GearboxType>(v)
            );
        
        modelBuilder.Entity<CarEntity>().Property(e => e.AirConditioningType)
            .HasConversion(
                v => v.ToString(),
                v => CommonMethods.StringToEnum<AirConditioningType>(v)
            );
        
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

        modelBuilder.Entity<CarEntity>().HasData(
            new CarEntity
            {
                Id = 1,
                Brand = "Mercedes-Benz",
                Model = "G",
                Colour = "Black",
                FuelType = FuelType.Petrol,
                GearboxType = GearboxType.Manual,
                AirConditioningType = AirConditioningType.Automatic,
                ProductionYear = 2010,
                NumberOfSeats = 5,
                FuelUsage = 11.2,
                PricePerDay = 150,
                Availability = true,
                RegistrationDate = DateTime.UtcNow
            },
            new CarEntity
            {
                Id = 2,
                Brand = "Toyota",
                Model = "Corolla",
                Colour = "White",
                FuelType = FuelType.Petrol,
                GearboxType = GearboxType.Manual,
                AirConditioningType = AirConditioningType.Manual,
                ProductionYear = 2007,
                NumberOfSeats = 5,
                FuelUsage = 6.5,
                PricePerDay = 40,
                Availability = true,
                RegistrationDate = DateTime.UtcNow
            },
            new CarEntity
            {
                Id = 3,
                Brand = "Hyundai",
                Model = "i30",
                Colour = "Red",
                FuelType = FuelType.Petrol,
                GearboxType = GearboxType.Automatic,
                AirConditioningType = AirConditioningType.Automatic,
                ProductionYear = 2022,
                NumberOfSeats = 5,
                FuelUsage = 5.1,
                PricePerDay = 65,
                Availability = true,
                RegistrationDate = DateTime.UtcNow
            }
            );
    }
}