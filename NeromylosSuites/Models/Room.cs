namespace NeromylosSuites.Models
{
    public class Room : BaseEntity
    {
        public int RoomNumber { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int MaxOccupancy { get; set; } = 0!;
        public string Status { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
        public ICollection<SeasonalPrice>? SeasonalPrices { get; set; } = new List<SeasonalPrice>();
    }
}
