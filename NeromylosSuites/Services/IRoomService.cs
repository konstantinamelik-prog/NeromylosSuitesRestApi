using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;

namespace NeromylosSuites.Services
{
    public interface IRoomService
    {
        Task<List<BookingReadOnlyDTO>> GetRoomBookingsAsync(string roomName);
        Task<List<RoomReadOnlyDTO>> GetAvailableRoomsByDateRangeAsync(DateTime checkIn, DateTime checkOut);
        Task<PaginatedResult<RoomReadOnlyDTO>> GetPaginatedRoomsFilteredAsync(
            int pageNumber, int pageSize, RoomFiltersDTO roomFiltersDTO);
    }
}
