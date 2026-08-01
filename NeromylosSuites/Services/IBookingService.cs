using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;

namespace NeromylosSuites.Services
{
    public interface IBookingService
    {
        Task<BookingReadOnlyDTO> CreateBookingAsync(CreateBookingDTO request);
        Task DeleteBookingAsync(int bookingId);
        Task<BookingReadOnlyDTO> GetBookingByIdAsync(int bookingId);
        Task<List<BookingReadOnlyDTO>> GetBookingsByUserIdAsync(int userId);
        Task<List<BookingReadOnlyDTO>> GetBookingsByVisitorIdAsync(int visitorId);
        Task<PaginatedResult<BookingReadOnlyDTO>> GetPaginatedBookingsFilteredAsync
            (int pageNumber, int pageSize, BookingFiltersDTO bookingFiltersDTO);
    }
}
