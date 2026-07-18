using NeromylosSuites.Core;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<List<Room>> GetRoomsByIdsAsync(List<int> roomIds);
        Task<List<Booking>> GetRoomBookingsAsync(string roomName);
        Task<List<Room>> GetAvailableRoomsByDateRangeAsync(DateTime checkIn, DateTime checkOut);
        Task<PaginatedResult<Room>> GetPaginatedRoomsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Room, bool>>> predicates);
    }
}
