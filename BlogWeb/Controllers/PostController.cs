using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWeb.Controllers
{
    public class PostController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _token;
        private readonly string _baseUrl;

        public PostController(ILogger<HomeController> logger, IConfiguration configuration, ICategoryService categoryService, IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _baseUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
            _categoryService = categoryService;
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
            _token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Article(int id) { 

            var postResponse = await _postService.GetAsync<APIResponse>(id);
            var post = JsonConvert.DeserializeObject<PostDTO>(Convert.ToString(postResponse.Result));
			ViewBag.Post = post;
			return View();

        }
    }
}
