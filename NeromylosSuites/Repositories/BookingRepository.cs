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
                .Where(b => !b.IsDeleted)
                .Include(b => b.User)
                .Include(b => b.Visitor)
                .Include(b => b.Rooms)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<List<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            List<Booking> bookings = await _context.Bookings
                .Where(b => !b.IsDeleted && b.UserId == userId)
                .Include(b => b.User)
                .Include(b => b.Rooms)
                .ToListAsync();

            return bookings;
        }

        public async Task<List<Booking>> GetBookingsByVisitorIdAsync(int visitorId)
        {
            List<Booking> bookings = await _context.Bookings
                .Where(b => !b.IsDeleted && b.VisitorId == visitorId)
                .Include(b => b.Visitor)
                .Include(b => b.Rooms)
                .ToListAsync();

            return bookings;
        }

        public async Task<PaginatedResult<Booking>> GetPaginatedBookingsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Booking, bool>>> predicates, string? sortBy, bool sortDescending)
        {
            IQueryable<Booking> query = _context.Bookings
                .Where(b => !b.IsDeleted)
                .Include(b => b.Rooms)
                .Include(b => b.User)
                .Include(b => b.Visitor);

            if (predicates != null && predicates.Count > 0)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            int totalRecords = await query.CountAsync();

            query = sortBy switch
            {
                "id" => sortDescending ? query.OrderByDescending(b => b.Id) : query.OrderBy(b => b.Id),
                "checkIn" => sortDescending ? query.OrderByDescending(b => b.CheckIn) : query.OrderBy(b => b.CheckIn),
                "checkOut" => sortDescending ? query.OrderByDescending(b => b.CheckOut) : query.OrderBy(b => b.CheckOut),
                "numberOfGuests" => sortDescending ? query.OrderByDescending(b => b.NumberOfGuests) : query.OrderBy(b => b.NumberOfGuests),
                "totalPrice" => sortDescending ? query.OrderByDescending(b => b.TotalPrice) : query.OrderBy(b => b.TotalPrice),
                "status" => sortDescending ? query.OrderByDescending(b => b.Status) : query.OrderBy(b => b.Status),
                _ => query.OrderBy(b => b.Id)  // default, ασφαλές fallback
            };

            int skip = (pageNumber - 1) * pageSize;

            var data = await query
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

        public async Task<bool> HasActiveBookingsForUserAsync(int userId) =>
            await _context.Bookings
                .AnyAsync(b => !b.IsDeleted && b.UserId == userId
                    && BookingStatuses.ActiveStatuses.Contains(b.Status));

        public async Task<bool> HasActiveBookingsForVisitorAsync(int visitorId) =>
            await _context.Bookings
                .AnyAsync(b => !b.IsDeleted && b.VisitorId == visitorId
                            && BookingStatuses.ActiveStatuses.Contains(b.Status));
    }
}