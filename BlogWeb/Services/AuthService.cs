using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;

namespace BlogWeb.Services
{
	public class AuthService : BaseService, IAuthService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly string _appUrl;

		public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
			_appUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
		}

		public Task<T> LoginAsync<T>(LoginRequestDTO loginDTO)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = loginDTO,
				Url = _appUrl + "/api/authAPI",
			});
		}
	}
}
