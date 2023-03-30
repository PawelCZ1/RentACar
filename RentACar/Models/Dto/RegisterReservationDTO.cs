namespace RentACar.Models.Dto;

public class RegisterReservationDTO
{
    public int CustomerId { get; set; }
    public int CarId { get; set; }
    public int Days { get; set; }
}