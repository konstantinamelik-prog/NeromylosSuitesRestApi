namespace NeromylosSuites.Models;

public class Booking : BaseEntity
{
    public int Id { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string? SpecialRequests { get; set; }
    public string Status { get; set; } = null!;
    public int? UserId { get; set; }
    public int? VisitorId { get; set; }
    public User? User { get; set; }
    public Visitor? Visitor { get; set; }
    public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
}
