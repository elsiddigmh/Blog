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


		[HttpGet]
		[Route("Contact")]
		public IActionResult Contact()
		{
			return View();
		}

		[HttpGet]
		[Route("About")]
		public IActionResult About()
		{
			return View();
		}

		[HttpGet]
        [Route("PrivacyPolicy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [HttpGet]
        [Route("TermsConditions")]
        public IActionResult TermsConditions()
        {
            return View();
        }

        [HttpGet]
        [Route("Page404")]
        public IActionResult Page404()
        {
            return View();
        }


    }
}
