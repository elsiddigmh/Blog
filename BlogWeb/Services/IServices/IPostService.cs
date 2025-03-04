using BlogWeb.Models.Dto;

namespace BlogWeb.Services.IServices
{
    public interface IPostService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(PostCreateDTO postDTO, IFormFile file, string token);
        Task<T> UpdateAsync<T>(PostUpdateDTO postDTO, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }


}
