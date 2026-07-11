using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;

namespace NeromylosSuites.Services
{
    public interface IVisitorService
    {
        Task<VisitorReadOnlyDTO> CreateVisitorAsync(CreateVisitorDTO createVisitorDTO);
        Task<VisitorReadOnlyDTO> GetVisitorByPhoneNumberAsync(string phoneNumber);
        Task<VisitorReadOnlyDTO> GetVisitorByIdAsync(int id);
        Task<List<BookingReadOnlyDTO>> GetVisitorBookingsAsync(int visitorId);
        Task<List<VisitorReadOnlyDTO>> GetVisitorsByCountryCodeAsync(string countryCode);
        Task<PaginatedResult<VisitorReadOnlyDTO>> GetPaginatedVisitorsAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<VisitorReadOnlyDTO>> GetPaginatedVisitorsFilteredAsync(
            int pageNumber, int pageSize, VisitorFiltersDTO visitorFiltersDTO);
    }
}
