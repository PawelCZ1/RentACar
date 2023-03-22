using Investments.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Investments.Data;

public class ApiDatabaseContext : DbContext
{
    public ApiDatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<CustomerEntity> CustomerEntities { get; set; }
    public DbSet<CarEntity> CarEntities { get; set; }
    public DbSet<ReservationEntity> ReservationEntities { get; set; }
}