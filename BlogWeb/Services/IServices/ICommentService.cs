using BlogWeb.Models.Dto;

namespace BlogWeb.Services.IServices
{
    public interface ICommentService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CommentCreateDTO commentDTO);
        Task<T> UpdateAsync<T>(CommentUpdateDTO commentDTO);
        Task<T> DeleteAsync<T>(int id);
    }


}
