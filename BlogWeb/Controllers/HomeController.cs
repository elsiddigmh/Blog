using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Models.VM;
using BlogWeb.Services;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;

namespace BlogWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _token;
        private readonly string _baseUrl;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ICategoryService categoryService, IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _baseUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
            _categoryService = categoryService;
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
            _token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            // Pagination => 5 Posts in Page
            int pageSize = 5;

            var categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
            var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));

            var postResponse = await _postService.GetAllAsync<APIResponse>();
            var posts = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(postResponse.Result));

            var latestOne = posts.OrderByDescending(p => p.Id).FirstOrDefault();
            var latestPosts = posts.OrderByDescending(p => p.Id).Take(5).ToList();

            var paginatedPosts = posts.OrderByDescending(p => p.Id)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();

            //var pagination = new PostPaginationVM
            //{
            //    Posts = paginatedPosts,
            //    CurrentPage = page,
            //    TotalPages = (int)Math.Ceiling((double)posts.Count / pageSize)
            //};

            ViewData["baseUrl"] = _baseUrl;
            ViewData["latestOne"] = latestOne;

            ViewData["paginatedPosts"] = paginatedPosts;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)posts.Count / pageSize);



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
