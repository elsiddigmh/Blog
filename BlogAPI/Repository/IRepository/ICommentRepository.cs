using BlogAPI.Models;

namespace BlogAPI.Repository.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment> UpdateAsync(Comment entity);
    }
}
