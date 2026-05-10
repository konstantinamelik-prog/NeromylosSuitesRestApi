using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Core;
using NeromylosSuites.Data;
using NeromylosSuites.Models;
using System.Linq.Expressions;

namespace NeromylosSuites.Repositories
{
    public class VisitorRepository : BaseRepository<Visitor>, IVisitorRepository
    {
        public VisitorRepository(NeromylosSuitesMvcContext context) : base(context) 
        {
        }

        public async Task<Visitor?> GetVisitorByLastnameAsync(string lastname) =>
            await _context.Visitors.FirstOrDefaultAsync(v => v.Lastname == lastname);

        public async Task<Visitor?> GetVisitorByEmailAsync(string email) => 
            await _context.Visitors.FirstOrDefaultAsync(v => v.Email == email);

        public async Task<Visitor?> GetVisitorByPhoneNumberAsync(string phoneNumber) =>
            await _context.Visitors.FirstOrDefaultAsync(v => v.PhoneNumber == phoneNumber);

        public async Task<List<Booking>> GetVisitorBookingsAsync(int visitorId)
        {
            List<Booking> bookings;

            bookings = await _context.Visitors
                .Where(v => v.Id == visitorId)
                .SelectMany(b => b.Bookings)
                .ToListAsync();

            return bookings;
        }

        public async Task<PaginatedResult<Visitor>> GetPaginatedVisitorsAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            int totalRecords = await _context.Visitors.CountAsync();

            var visitors = await _context.Visitors
                .OrderBy(v => v.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Visitor>(visitors, totalRecords, pageNumber, pageSize);
        }

        public async Task<PaginatedResult<Visitor>> GetPaginatedVisitorsFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<Visitor, bool>>> predicates)
        {
            IQueryable<Visitor> query = _context.Visitors;

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
                .OrderBy(v => v.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Visitor>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
