using NeromylosSuites.Core;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public interface IVisitorRepository : IBaseRepository<Visitor>
    {
        Task<Visitor?> GetVisitorByEmailAsync(string email);
        Task<Visitor?> GetVisitorByPhoneNumberAsync(string phoneNumber);
        Task<List<Booking>> GetVisitorBookingsAsync(int visitorId);
        Task<List<Visitor>> GetVisitorsByCountryCodeAsync(string countryCode);
        Task<PaginatedResult<Visitor>> GetPaginatedVisitorsAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<Visitor>> GetPaginatedVisitorsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Visitor, bool>>> predicates);
    }
}
