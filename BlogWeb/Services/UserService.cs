using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;

namespace BlogWeb.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _appUrl;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public UserService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _appUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
        }

        public Task<T> CreateAsync<T>(UserCreateDTO userDTO)
        {
            return SendAsync<T>(new APIRequest { 
                ApiType = SD.ApiType.POST,
                Data = userDTO,
                Url = _appUrl + "/api/userAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = _appUrl + "/api/userAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = _appUrl + "/api/userAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = _appUrl + "/api/userAPI/" + id
            });
        }

        public Task<T> UpdateAsync<T>(UserUpdateDTO userDTO)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = userDTO,
                Url = _appUrl + "/api/userAPI/" + userDTO.Id
            });
        }
    }
}
