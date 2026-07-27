using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Data;
using NeromylosSuites.Models;

namespace NeromylosSuites.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly NeromylosSuitesMvcContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(NeromylosSuitesMvcContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            T? existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity is null) return false;
            existingEntity.IsDeleted = true;
            existingEntity.DeletedAt = DateTime.UtcNow;
            _context.Entry(existingEntity).State = EntityState.Modified;
            return true;
        }

        public virtual async Task<T?> GetByIdAsync(int id) =>
            await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        public virtual async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.Where(e => !e.IsDeleted).ToListAsync();

        public virtual async Task<int> GetCountAsync() =>
            await _dbSet.CountAsync(e => !e.IsDeleted);
    }
}
