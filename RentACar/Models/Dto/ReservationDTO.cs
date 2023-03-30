using RentACar.Models.Db;

namespace RentACar.Models.Dto;

public class ReservationDTO : BaseDTO
{
    public int CustomerEntityId { get; set; }
    public int CarEntityId { get; set; }
    public DateTime ReservationStartDate { get; set; }
    public DateTime ReservationFinishDate { get; set; }
}