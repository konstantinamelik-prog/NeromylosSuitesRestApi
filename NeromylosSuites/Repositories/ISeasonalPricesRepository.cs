using NeromylosSuites.Models;

namespace NeromylosSuites.Repositories
{
    public interface ISeasonalPricesRepository : IBaseRepository<SeasonalPrice>
    {
        Task<List<SeasonalPrice>> GetSeasonalPricesBySeasonNameAsync(string seasonName);
        Task<List<SeasonalPrice>> GetSeasonalPricesByRoomId(int roomId);
        Task<SeasonalPrice?> GetPriceForRoomAndDateAsync(int roomId, DateTime date);
    }
}
