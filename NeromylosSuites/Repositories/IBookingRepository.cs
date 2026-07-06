using NeromylosSuites.Core;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<List<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<List<Booking>> GetBookingsByVisitorIdAsync(int visitorId);
        Task<PaginatedResult<Booking>> GetPaginatedBookingsAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<Booking>> GetPaginatedBookingsFilteredAsync
            (int pageNumber, int pageSize, List<Expression<Func<Booking, bool>>> predicates);
    }
}
