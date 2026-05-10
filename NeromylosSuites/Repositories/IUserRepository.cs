using NeromylosSuites.Core;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByLastnameAsync(string lastname);
        Task<List<Booking>> GetUserBookingsAsync(int userId);
        Task<PaginatedResult<User>> GetPaginatedUsersAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates);
    }
}