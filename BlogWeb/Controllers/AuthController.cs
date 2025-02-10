using AutoMapper;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWeb.Controllers
{
	public class AuthController : Controller
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		public AuthController(IUserService userService, IMapper mapper)
		{
			_userService = userService;
			_mapper = mapper;
		}

		[HttpGet]
		[Route("Register")]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register(UserCreateDTO userDTO)
		{
			var chcekEmailResponse = await IsEmailAvailable(userDTO.Email) as JsonResult;
            bool isEmailAvailable = (bool)chcekEmailResponse.Value;
			if (isEmailAvailable) {
				ModelState.AddModelError("Email", "Email already exists!");
			}

			var chcekUsernameResponse = await IsUsernameAvailable(userDTO.UserName) as JsonResult;
			bool isUsernameAvailable = (bool)chcekUsernameResponse.Value;
			if (isUsernameAvailable)
			{
				ModelState.AddModelError("UserName", "Username already exists!");
			}

			if (ModelState.IsValid)
			{
				var response = await _userService.CreateAsync<APIResponse>(userDTO);
				if (response != null && response.IsSuccess) {
					TempData["success"] = "Your account has been created!";
					return RedirectToAction(nameof(Login));
				}
			}
			//TempData["error"] = "Something went wrong!";
			return View(userDTO);

		}

		//Unique email check for Remote validation
	   [HttpGet]
		public async Task<IActionResult> IsEmailAvailable(string email)
		{
			var response = await _userService.GetAllAsync<APIResponse>();
			var users = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
			bool result;
			foreach (var user in users) {
				if (user.Email == email) { 
					result = true;
                    return Json(result);
                }
			}
			result = false;
            return Json(result);
        }

		//Unique username check for Remote validation
		[HttpGet]
		public async Task<IActionResult> IsUsernameAvailable(string username)
		{
			var response = await _userService.GetAllAsync<APIResponse>();
			var users = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
			bool result;
			foreach (var user in users)
			{
				if (user.UserName == username)
				{
					result = true;
					return Json(result);
				}
			}
			result = false;
			return Json(result);
		}




		[HttpGet]
		[Route("Login")]
		public IActionResult Login()
		{
			return View();
		}

	}
}
