using NeromylosSuites.Core;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public interface IMemberRepository : IBaseRepository<Member>
    {
        Task<Member?> GetMemberByPhoneNumberAsync(string phonenumber);
        Task<User?> GetUserMemberByUsernameAsync(string username);
        Task<List<Booking>> GetMemberBookingsAsync(int userId);
        Task<List<Member>> GetMembersByCountryCodeAsync(string countryCode);
        Task<PaginatedResult<User>> GetPaginatedUsersMembersFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates);
    }
}
