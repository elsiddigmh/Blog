﻿using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace BlogWeb.Services
{
    public class BaseService : IBaseService
    {

        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
		private readonly IHttpContextAccessor _httpContextAccessor;

		public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClient = httpClient;
            this.responseModel = new APIResponse();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("BlogAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);

				// Handle multipart/form-data
				if (apiRequest.Data is MultipartFormDataContent formData)
				{
					message.Content = formData;
				}
				else
				{
					// Default JSON handling
					if (apiRequest.Data != null)
					{
						message.Content = new StringContent(
							JsonConvert.SerializeObject(apiRequest.Data),
							Encoding.UTF8,
							"application/json"
						);
					}
				}

                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;
				var tokenFromCookie = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
				if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenFromCookie);
                }
                apiResponse = await client.SendAsync(message);
                
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (ApiResponse != null && (apiResponse.StatusCode == HttpStatusCode.BadRequest 
                                                || apiResponse.StatusCode == HttpStatusCode.NotFound)){ 

                        ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;
                        var response = JsonConvert.SerializeObject(ApiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(response);
                        return returnObj;
                    }

                }catch(Exception ex)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex) {
                var dto = new APIResponse()
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false,
                };

                var response = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(response);
                return APIResponse;
            }
        }
    }
}
