using AutoMapper;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace BlogWeb.Areas.Management.Controllers
{
	[Area("Management")]
	public class ProfileController : Controller
	{
		private readonly IUserService _userService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly string _token;
		public ProfileController(IUserService userService, IHttpContextAccessor httpContextAccessor)
		{
			_userService = userService;
			_httpContextAccessor = httpContextAccessor;
			_token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
		}
		public async Task<IActionResult> Index()
		{
			var usersResponse = await _userService.GetAllAsync<APIResponse>();
			var users = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(usersResponse.Result));
			UserDTO userProfile;
			if (!string.IsNullOrEmpty(_token)) { 
			
				var handler = new JwtSecurityTokenHandler();
				var jwtToken = handler.ReadJwtToken(_token);
				var email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

				foreach (var user in users) {
					if (user.Email == email) { 
						userProfile = user;
						return View(userProfile);
					}
				}
			}

			return NotFound();

		}


	}
}
