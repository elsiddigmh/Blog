using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Repository.IRepository;

namespace BlogAPI.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Category> UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
