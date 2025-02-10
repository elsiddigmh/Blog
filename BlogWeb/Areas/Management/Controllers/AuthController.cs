using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.Areas.Management.Controllers
{
	public class AuthController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
