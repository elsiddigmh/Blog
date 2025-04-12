using BlogWeb.Models.Dto;

namespace BlogWeb.Services.IServices
{
    public interface IPostService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(PostCreateDTO postDTO, string token);
        Task<T> UpdateAsync<T>(PostUpdateDTO postDTO, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }


}
