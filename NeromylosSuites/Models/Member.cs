namespace NeromylosSuites.Models;

public class Member : BaseEntity
{
    public string? CountryCode { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public int UserId { get; set; }
    public required User User { get; set; }
}
