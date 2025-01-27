using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Repository.IRepository;

namespace BlogAPI.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Comment> UpdateAsync(Comment entity)
        {
            _context.Comments.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
