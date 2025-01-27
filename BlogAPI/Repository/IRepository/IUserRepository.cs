using BlogAPI.Models;

namespace BlogAPI.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> UpdateAsync(User entity);
    }
}
