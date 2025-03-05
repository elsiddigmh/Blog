using AutoMapper;
using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlogWeb.Controllers
{
	public class AuthController : Controller
	{
		private readonly IUserService _userService;
		private readonly IAuthService _authService;
		private readonly IMapper _mapper;
		public AuthController(IUserService userService, IMapper mapper, IAuthService authService)
		{
			_userService = userService;
			_mapper = mapper;
			_authService = authService;
		}

		[HttpGet]
		[Route("Register")]
		public IActionResult Register()
		{
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

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


		[HttpGet]
		[Route("Login")]
		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated) { 
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login(LoginRequestDTO loginDTO)
		{
			if (!ModelState.IsValid) { 
				return View(loginDTO);
			}
			APIResponse response = await _authService.LoginAsync<APIResponse>(loginDTO);
			if (response != null && response.IsSuccess)
			{
				LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));	
				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(model.Token);
				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
				identity.AddClaim(new Claim(ClaimTypes.Email, jwt.Claims.FirstOrDefault(u=>u.Type == "email").Value));
				identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=>u.Type == "role").Value));
				var principle = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

				// Store Token in session
				//HttpContext.Session.SetString(SD.SessionToken, model.Token);
				//var token = HttpContext.Session.GetString(SD.SessionToken);

				// Store Token in cookie
				var cookieOptions = new CookieOptions
				{
					HttpOnly = true, // Prevent JavaScript access (XSS protection)
					Secure = true,   // Only transmit over HTTPS
					SameSite = SameSiteMode.Strict, // Prevent CSRF attacks
					Expires = DateTime.UtcNow.AddMinutes(60) // Expiry time
				};

				Response.Cookies.Append("AuthToken", model.Token, cookieOptions);

				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
				TempData["error"] = "Username or password is incorrect!";
				return View(loginDTO);
			}

		}


		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			//Using Session
			//HttpContext.Session.SetString(SD.SessionToken, "");

			// Using Cookie
			Response.Cookies.Delete("AuthToken");
			return RedirectToAction("Index", "Home");
		}

		[Route("AccessDenied")]
		public IActionResult AccessDenied()
		{
			return View();
		}




		//Unique email check for Remote validation
		[HttpGet]
		public async Task<IActionResult> IsEmailAvailable(string email)
		{
			var response = await _userService.GetAllAsync<APIResponse>();
			var users = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
			bool result;
			if (users != null) {
				foreach (var user in users)
				{
					if (user.Email == email)
					{
						result = true;
						return Json(result);
					}
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
			if (users != null)
			{
				foreach (var user in users)
				{
					if (user.UserName == username)
					{
						result = true;
						return Json(result);
					}
				}
			}
			result = false;
			return Json(result);
		}

	}
}
