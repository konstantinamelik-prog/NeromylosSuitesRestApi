namespace NeromylosSuites.Models
{
    public class SeasonalPrice : BaseEntity
    {
        public string SeasonName { get; set; } = null!;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Price { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
