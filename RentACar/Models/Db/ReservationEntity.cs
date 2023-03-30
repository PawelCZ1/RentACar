namespace RentACar.Models.Db;

public class ReservationEntity : BaseEntity
{
    public int CustomerEntityId { get; set; }
    public virtual CustomerEntity CustomerEntity { get; set; }
    public int CarEntityId { get; set; }
    public virtual CarEntity CarEntity { get; set; }
    public DateTime ReservationStartDate { get; set; }
    public DateTime ReservationFinishDate { get; set; }
}