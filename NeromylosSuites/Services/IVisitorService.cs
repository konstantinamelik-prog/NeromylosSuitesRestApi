using NeromylosSuites.Core;
using NeromylosSuites.DTO;

namespace NeromylosSuites.Services
{
    public interface IVisitorService
    {
        Task<VisitorReadOnlyDTO> CreateVisitorAsync(CreateVisitorDTO request);

        Task<PaginatedResult<VisitorReadOnlyDTO>> GetPaginatedVisitorsAsync(int pageNumber, int pageSize);
    }
}
