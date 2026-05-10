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

        public async Task<User?> GetBookingUserAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Where(b => b.Id == bookingId && b.User != null)
                .FirstOrDefaultAsync();

            return booking?.User;
        }

        public async Task<Visitor?> GetBookingVisitorAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Visitor)
                .Where(b => b.Id == bookingId && b.Visitor != null)
                .FirstOrDefaultAsync();

            return booking?.Visitor;
        }

        public async Task<List<Room>> GetBookingRoomsAsync(int bookingId)
        {
            return await _context.Bookings
                .Where(b => b.Id == bookingId)
                .SelectMany(b => b.Rooms)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByCheckInAsync(DateTime checkIn)
        {
            return await _context.Bookings
                .Where(b => b.CheckIn == checkIn)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByCheckOutAsync(DateTime checkOut)
        {
            return await _context.Bookings
                .Where(b => b.CheckOut == checkOut)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByStatusAsync(string status)
        {
            return await _context.Bookings
                .Where(b => b.Status == status)
                .ToListAsync();
        }

        public async Task<PaginatedResult<Booking>> GetPaginatedBookingsAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            int totalRecords = await _context.Bookings.CountAsync();

            var bookings = await _context.Bookings
                .OrderBy(b => b.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Booking>(bookings, totalRecords, pageNumber, pageSize);
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
