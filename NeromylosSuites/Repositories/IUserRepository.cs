using NeromylosSuites.Core;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserWithMemberByIdAsync(int id);
        Task<PaginatedResult<User>> GetPaginatedUsersAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates);
    }
}