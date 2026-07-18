using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Data;
using NeromylosSuites.Models;

namespace NeromylosSuites.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(NeromylosSuitesMvcContext context) : base(context)
        {
        }

        public async Task<List<Booking?>> GetRoomBookingsAsync(int roomId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.Rooms.Any(r => r.Id == roomId))
                .Select(b => (Booking?)b)
                .ToListAsync();

            return bookings;
        }

        public async Task<List<Room>> GetAvailableRoomsByDateRangeAsync(DateTime checkIn, DateTime checkOut)
        {
            var bookedRoomIds = await _context.Bookings
                .Where(b => b.CheckIn < checkOut && b.CheckOut > checkIn
                            && b.Status != "CANCELLED")
                .SelectMany(b => b.Rooms)
                .Select(r => r.Id)
                .ToListAsync();

            return await _context.Rooms
                .Where(r => !bookedRoomIds.Contains(r.Id))
                .ToListAsync();
        }
    }
}
