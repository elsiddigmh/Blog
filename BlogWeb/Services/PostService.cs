using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;

namespace BlogWeb.Services
{
    public class PostService : BaseService, IPostService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _appUrl;

        public PostService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _appUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
        }

        public Task<T> CreateAsync<T>(PostCreateDTO postDTO)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.POST,
                Data = postDTO,
                Url = _appUrl + "/api/postAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = _appUrl + "/api/postAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = _appUrl + "/api/postAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = _appUrl + "/api/postAPI/" + id
            });
        }

        public Task<T> UpdateAsync<T>(PostUpdateDTO postDTO)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = postDTO,
                Url = _appUrl + "/api/postAPI/" + postDTO.Id
            });
        }
    }
}
