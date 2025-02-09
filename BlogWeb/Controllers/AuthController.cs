using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.Controllers
{
	public class AuthController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

	}
}
