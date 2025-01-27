using BlogAPI.Data;
using BlogAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context) { 
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string[]? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string[]? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if (tracked)
            {
                query = query.AsNoTracking();
            }
            if(includeProperties != null)
            {
                foreach(var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
             dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
