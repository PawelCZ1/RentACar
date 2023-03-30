using System.Text.Json.Serialization;
using RentACar.Models.Enum;
using RentACar.Utils;

namespace RentACar.Models.Dto;

public class CarDTO : BaseDTO
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Colour { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FuelType FuelType { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GearboxType GearboxType { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AirConditioningType AirConditioningType { get; set; }
    public int ProductionYear { get; set; }
    public byte NumberOfSeats { get; set; }
    public double FuelUsage { get; set; }
    public double PricePerDay { get; set; }
}