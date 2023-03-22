using System.ComponentModel.DataAnnotations;

namespace Investments.Models.Db;

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
    public virtual ICollection<ReservationEntity> ReservationEntities { get; set; }


}