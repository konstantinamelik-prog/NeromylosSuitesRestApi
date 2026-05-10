using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Data;
using NeromylosSuites.Models;

namespace NeromylosSuites.Repositories
{
    public class SeasonalPricesRepository : BaseRepository<SeasonalPrice>, ISeasonalPricesRepository
    {
        public SeasonalPricesRepository(NeromylosSuitesMvcContext context) : base(context)
        {
        }

        public async Task<List<SeasonalPrice>> GetSeasonalPricesBySeasonNameAsync(string seasonName) =>
            await _context.SeasonalPrices
            .Where(s => s.SeasonName == seasonName)
            .ToListAsync();

        public async Task<List<SeasonalPrice>> GetSeasonalPricesByRoomId(int roomId) =>
            await _context.SeasonalPrices
            .Where(sp => sp.RoomId == roomId)
            .ToListAsync();

        public async Task<SeasonalPrice?> GetPriceForRoomAndDateAsync(int roomId, DateTime date) =>
            await _context.SeasonalPrices
                .Where(sp => sp.RoomId == roomId 
                    && sp.DateFrom <= date
                    && sp.DateTo >= date)
                .FirstOrDefaultAsync();
    }
}
