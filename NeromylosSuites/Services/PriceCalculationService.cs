using NeromylosSuites.Exceptions;
using NeromylosSuites.Repositories;

namespace NeromylosSuites.Services
{
    public class PriceCalculationService : IPriceCalculationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriceCalculationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> CalculateRoomPriceAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            decimal totalPrice = 0;
            var currentDate = checkIn;

            while (currentDate < checkOut)
            {
                var seasonalPrice = await _unitOfWork.SeasonalPricesRepository
                    .GetPriceForRoomAndDateAsync(roomId, currentDate);

                if (seasonalPrice == null)
                {
                    throw new EntityNotFoundException("SeasonalPrice",
                        $"No price found for roomId {roomId} on {currentDate:dd/MM/yyyy}");
                }

                totalPrice += seasonalPrice.Price;
                currentDate = currentDate.AddDays(1);
            }

            return totalPrice;
        }
    }
}
