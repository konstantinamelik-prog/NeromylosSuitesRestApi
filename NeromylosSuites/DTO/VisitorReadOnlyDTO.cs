namespace NeromylosSuites.DTO
{
    public record VisitorReadOnlyDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? CountyCode { get; set; }
    }
}
