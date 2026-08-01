using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Core;
using NeromylosSuites.Data;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(NeromylosSuitesMvcContext context) : base(context)
        {
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Visitor)
                .Include(b => b.Rooms)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<List<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            List<Booking> bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Visitor)
                .Include(b => b.Rooms)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return bookings;
        }

        public async Task<List<Booking>> GetBookingsByVisitorIdAsync(int visitorId)
        {
            List<Booking> bookings = await _context.Bookings
                .Include(b => b.Visitor)
                .Include(b => b.Rooms)
                .Where(b => b.VisitorId == visitorId)
                .ToListAsync();

            return bookings;
        }

        public async Task<PaginatedResult<Booking>> GetPaginatedBookingsFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<Booking, bool>>> predicates)
        {
            IQueryable<Booking> query = _context.Bookings;

            if (predicates != null && predicates.Count > 0)
            {
                foreach(var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            int totalRecords = await query.CountAsync();

            int skip = (pageNumber - 1) * pageSize;

            var data = await query
                .OrderBy(b => b.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Booking>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
