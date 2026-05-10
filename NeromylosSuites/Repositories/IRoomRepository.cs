using NeromylosSuites.Models;

namespace NeromylosSuites.Repositories
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<List<Booking?>> GetRoomBookingsAsync(int roomId);
        Task<List<Room>> GetAvailableRoomsByDateRangeAsync(DateTime checkIn, DateTime checkOut);
    }
}
