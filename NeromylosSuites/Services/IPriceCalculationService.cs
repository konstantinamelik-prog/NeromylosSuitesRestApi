namespace NeromylosSuites.Services
{
    public interface IPriceCalculationService
    {
        Task<decimal> CalculateRoomPriceAsync(int roomId, DateTime checkIn, DateTime checkOut);
    }
}