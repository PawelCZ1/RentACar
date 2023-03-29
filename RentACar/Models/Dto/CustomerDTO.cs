namespace RentACar.Models.Dto;

public class CustomerDTO : BaseDTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Nationality { get; set; }
    public double Balance { get; set; }
    public DateTime BirthDate { get; set; }
}