using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Core;
using NeromylosSuites.Data;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(NeromylosSuitesMvcContext context) : base (context)
        {
        }

        public async Task<User?> GetUserByIdAsync(int id) =>
            await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetUserByUsernameAsync(string username) => 
            await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username || u.Email == username);

        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _context.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(e => e.Email == email);

        public async Task<User?> GetUserWithMemberByIdAsync(int id) =>
            await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Member)
            .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<PaginatedResult<User>> GetPaginatedUsersFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            int totalRecords;
            IQueryable<User> query = _context.Users;

            if (predicates != null && predicates.Count > 0)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }
            totalRecords = await query.CountAsync();
            int skip = (pageNumber - 1) * pageSize;

            var data = await query
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<User>()
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}