namespace NeromylosSuites.Models;

public class Visitor : BaseEntity
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? CountryCode { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
}
