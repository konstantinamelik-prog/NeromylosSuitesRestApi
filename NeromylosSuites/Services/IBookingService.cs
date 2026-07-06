using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;

namespace NeromylosSuites.Services
{
    public interface IBookingService
    {
        Task<BookingReadOnlyDTO> CreateBookingAsync(CreateBookingDTO createBooking);
        Task<BookingReadOnlyDTO> GetBookingByIdAsync(int bookingId);
        Task<List<BookingReadOnlyDTO>> GetBookingsByUserIdAsync(int userId);
        Task<List<BookingReadOnlyDTO>> GetBookingsByVisitorIdAsync(int visitorId);
        Task<PaginatedResult<BookingReadOnlyDTO>> GetPaginatedBookingsAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<BookingReadOnlyDTO>> GetPaginatedBookingsFilteredAsync
            (int pageNumber, int pageSize, BookingFiltersDTO bookingFiltersDTO);
    }
}
