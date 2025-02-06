using BlogWeb.Models.Dto;

namespace BlogWeb.Services.IServices
{
    public interface ICategoryService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CategoryCreateDTO categoryDTO);
        Task<T> UpdateAsync<T>(CategoryUpdateDTO categoryDTO);
        Task<T> DeleteAsync<T>(int id);
    }


}
