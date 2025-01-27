using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Repository.IRepository;

namespace BlogAPI.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<Post> UpdateAsync(Post entity)
        {
            _context.Posts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
