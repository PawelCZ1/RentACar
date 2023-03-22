using System.ComponentModel.DataAnnotations;
using Investments.Models.Enum;

namespace Investments.Models.Db;

public class CarEntity : BaseEntity
{
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public string Colour { get; set; }
    [Required]
    public FuelType FuelType { get; set; }
    [Required]
    public GearboxType GearboxType { get; set; }
    [Required]
    public AirConditioningType AirConditioningType { get; set; }
    [Required]
    public DateTime ProductionDate { get; set; }
    [Required]
    public byte NumberOfSeats { get; set; }
    [Required]
    public double FuelUsage { get; set; }
    [Required]
    public double PricePerDay { get; set; }
    [Required]
    public bool Availability { get; set; }
    public virtual ICollection<ReservationEntity> ReservationEntities { get; set; }
    
    
    
    
    
}