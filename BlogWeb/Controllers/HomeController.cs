using BlogWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

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
