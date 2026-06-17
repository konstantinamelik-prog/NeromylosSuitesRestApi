namespace NeromylosSuites.Core.Filters
{
    public class SeasonalPricesFiltersDTO
    {
        public string? SeasonName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
