using NeromylosSuites.Models;

namespace NeromylosSuites.DTO
{
    public class RoomReadOnlyDTO
    {
        public int Id { get; set; }
        public int? RoomNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? MaxOccupancy { get; set; }
        public int? Status { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Price { get; set; }
    }
}
