namespace NeromylosSuites.DTO
{
    public record RoomReadOnlyDTO
    {
        public int Id { get; set; }
        public int? RoomNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? MaxOccupancy { get; set; }
        public string? Status { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
