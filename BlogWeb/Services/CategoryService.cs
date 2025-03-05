using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;

namespace BlogWeb.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _appUrl;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CategoryService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _appUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
        }
        public Task<T> CreateAsync<T>(CategoryCreateDTO categoryDTO)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.POST,
                Data = categoryDTO,
                Url = _appUrl + "/api/categoryAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = _appUrl + "/api/categoryAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = _appUrl + "/api/categoryAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = _appUrl + "/api/categoryAPI/" + id
            });
        }

        public Task<T> UpdateAsync<T>(CategoryUpdateDTO categoryDTO)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = categoryDTO,
                Url = _appUrl + "/api/categoryAPI/" + categoryDTO.Id
            });
        }
    }
}
