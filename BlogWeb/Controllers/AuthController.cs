using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.Controllers
{
	public class AuthController : Controller
	{
		[HttpGet]
		[Route("Register")]
		public IActionResult Register()
		{
			return View();
		}




		[HttpGet]
		[Route("Login")]
		public IActionResult Login()
		{
			return View();
		}

	}
}
