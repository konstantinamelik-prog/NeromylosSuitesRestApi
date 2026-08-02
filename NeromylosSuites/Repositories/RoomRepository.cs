using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Core;
using NeromylosSuites.Data;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(NeromylosSuitesMvcContext context) : base(context)
        {
        }

        public async Task<List<Room>> GetRoomsByIdsAsync(List<int> roomIds) =>
            await _context.Rooms
            .Where(r => roomIds.Contains(r.Id))
            .ToListAsync();

        public async Task<List<Booking>> GetRoomBookingsAsync(string roomName)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Rooms)
                .Where(b => b.Rooms.Any(r => r.Name == roomName))
                .ToListAsync();

            return bookings;
        }

        public async Task<List<Room>> GetAvailableRoomsByDateRangeAsync(DateTime checkIn, DateTime checkOut)
        {
            var bookedRoomIds = await _context.Bookings
                .Where(b => !b.IsDeleted
                            && b.CheckIn < checkOut && b.CheckOut > checkIn
                            && BookingStatuses.ActiveStatuses.Contains(b.Status))
                .SelectMany(b => b.Rooms)
                .Select(r => r.Id)
                .ToListAsync();

            return await _context.Rooms
                .Where(r => !bookedRoomIds.Contains(r.Id))
                .ToListAsync();
        }

        public async Task<PaginatedResult<Room>> GetPaginatedRoomsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Room, bool>>> predicates)
        {
            IQueryable<Room> query = _context.Rooms;

            if (predicates != null && predicates.Count > 0)
                foreach (var predicate in predicates)
                    query = query.Where(predicate);

            int totalRecords = await query.CountAsync();
            int skip = (pageNumber - 1) * pageSize;

            var data = await query
                .OrderBy(r => r.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Room>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
