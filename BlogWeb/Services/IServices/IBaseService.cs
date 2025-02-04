using BlogWeb.Models;

namespace BlogWeb.Services.IServices
{
    public interface IBaseService
    {
        public APIResponse responseModel { get; set; }
        public Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
