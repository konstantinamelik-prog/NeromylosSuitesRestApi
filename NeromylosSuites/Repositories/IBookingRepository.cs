using NeromylosSuites.Core;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<User?> GetBookingUserAsync(int bookingId);
        Task<Visitor?> GetBookingVisitorAsync(int bookingId);
        Task<List<Room>> GetBookingRoomsAsync(int bookingId);
        Task<List<Booking>> GetBookingsByCheckInAsync(DateTime checkIn);
        Task<List<Booking>> GetBookingsByCheckOutAsync(DateTime checkOut);
        Task<List<Booking>> GetBookingsByStatusAsync(string status);
        Task<PaginatedResult<Booking>> GetPaginatedBookingsAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<Booking>> GetPaginatedBookingsFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<Booking, bool>>> predicates);
    }
}
