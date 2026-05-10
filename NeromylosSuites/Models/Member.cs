namespace NeromylosSuites.Models;

public class Member : BaseEntity
{
    public int Id { get; set; }

    public string? CountryCode { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
