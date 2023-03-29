using System.ComponentModel.DataAnnotations;
using RentACar.Models.Enum;

namespace RentACar.Models.Db;

public class CarEntity : BaseEntity
{
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public string Colour { get; set; }
    [Required]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public FuelType FuelType { get; set; }
    [Required]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public GearboxType GearboxType { get; set; }
    [Required]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
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