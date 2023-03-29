using RentACar.Models.Enum;

namespace RentACar.Models.Dto;

public class CarDTO : BaseDTO
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Colour { get; set; }
    public FuelType FuelType { get; set; }
    public GearboxType GearboxType { get; set; }
    public AirConditioningType AirConditioningType { get; set; }
    public DateTime ProductionDate { get; set; }
    public byte NumberOfSeats { get; set; }
    public double FuelUsage { get; set; }
    public double PricePerDay { get; set; }
}