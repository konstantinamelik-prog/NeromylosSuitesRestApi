namespace NeromylosSuites.DTO
{
    public class BookingReadOnlyDTO
    {
        public int Id { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? NumberOfGuests { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Status { get; set; }
        public List<string>? RoomNames { get; set; }
    }
}
