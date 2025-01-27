using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Repository.IRepository;

namespace BlogAPI.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
