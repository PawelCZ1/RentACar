using System.ComponentModel.DataAnnotations;

namespace RentACar.Models.Db;

public class CustomerEntity : BaseEntity
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    public string Nationality { get; set; }
    [Required]
    public double Balance { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime RegistrationDate { get; set; }
    public virtual ICollection<ReservationEntity> ReservationEntities { get; set; }


}