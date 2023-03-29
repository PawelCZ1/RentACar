namespace RentACar.Models.Dto;

public class ReservationDTO : BaseDTO
{
    public DateTime ReservationStartDate { get; set; }
    public DateTime ReservationFinishDate { get; set; }
}