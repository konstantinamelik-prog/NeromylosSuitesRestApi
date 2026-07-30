using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Core;
using NeromylosSuites.Data;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public class MemberRepository : BaseRepository<Member>, IMemberRepository
    {
        public MemberRepository(NeromylosSuitesMvcContext context) : base(context)
        {
        }

        public async Task<Member?> GetMemberByPhoneNumberAsync(string phonenumber) => 
            await _context.Members
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.PhoneNumber == phonenumber);

        public async Task<User?> GetUserMemberByUsernameAsync(string username)
        {
            var UserMember = await _context.Users
                .Include(u => u.Member)
                .Include(u => u.Role)
                .Where(u => u.Username == username && u.Member != null)
                .SingleOrDefaultAsync();

            return UserMember;
        }

        public async Task<List<Booking>> GetMemberBookingsAsync(int userId)
        {
            List<Booking> bookings;

            bookings = await _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Bookings)
                .Include(b => b.Rooms)
                .ToListAsync();

            return bookings;
        }

        public async Task<List<Member>> GetMembersByCountryCodeAsync(string countryCode)
        {
            List<Member> members;

            members = await _context.Members
                .Where(m => m.CountryCode == countryCode)
                .ToListAsync();

            return members;
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersMembersFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            IQueryable<User> query = _context.Users
                .Include(u => u.Member)
                .Include(u => u.Role)
                .Where(u => u.Member != null);

            if (predicates != null && predicates.Count > 0)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }
            int totalRecords = await query.CountAsync();
            int skip = (pageNumber - 1) * pageSize;

            var data = await query
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<User>(data, totalRecords, pageNumber, pageSize);
        }
    }
}
