using BlogAPI.Models;

namespace BlogAPI.Repository.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> UpdateAsync(Post entity);
    }
}
