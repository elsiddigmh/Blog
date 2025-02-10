using AutoMapper;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;

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
			if (ModelState.IsValid)
			{
				var response = await _userService.CreateAsync<APIResponse>(userDTO);
				if (response != null && response.IsSuccess) {
					TempData["success"] = "Your account has been created!";
					return RedirectToAction(nameof(Login));
				}
			}

			TempData["error"] = "Something went wrong!";
			return View(userDTO);

		}




		[HttpGet]
		[Route("Login")]
		public IActionResult Login()
		{
			return View();
		}

	}
}
