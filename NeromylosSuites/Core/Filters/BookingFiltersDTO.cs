using System.Globalization;

namespace NeromylosSuites.Core.Filters
{
    public class BookingFiltersDTO
    {
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? Status { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public string? Lastname { get; set; }
    }
}
