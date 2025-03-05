using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using System.Net.Http.Headers;

namespace BlogWeb.Services
{
    public class PostService : BaseService, IPostService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _appUrl;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public PostService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _appUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<T> CreateAsync<T>(PostCreateDTO postDTO, IFormFile file, string token)
        {
            using (var formData = new MultipartFormDataContent())
            {
                // Add the file
                if (file != null)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                    formData.Add(fileContent, "File", file.FileName);
                }

                // Add other fields from postDTO
                foreach (var prop in postDTO.GetType().GetProperties())
                {
                    var value = prop.GetValue(postDTO);
                    if (value != null)
                    {
                        formData.Add(new StringContent(value.ToString()), prop.Name);
                    }
                }

                return SendAsync<T>(new APIRequest
                {
                    ApiType = SD.ApiType.POST,
                    Data = postDTO,
                    Url = _appUrl + "/api/postAPI",
                    Token = token
                });
            }
        }


		public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = _appUrl + "/api/postAPI/" + id,
				Token = token
			});
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = _appUrl + "/api/postAPI",
				Token = token
			});
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = _appUrl + "/api/postAPI/" + id,
				Token = token
			});
        }

        public Task<T> UpdateAsync<T>(PostUpdateDTO postDTO, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = postDTO,
                Url = _appUrl + "/api/postAPI/" + postDTO.Id,
				Token = token
			});
        }

    }
}
