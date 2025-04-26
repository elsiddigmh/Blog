using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWeb.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _token;
        private readonly string _baseUrl;

        public CategoryController(ILogger<HomeController> logger, IConfiguration configuration, ICategoryService categoryService, IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _baseUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
            _categoryService = categoryService;
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
            _token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
        }


        public async Task<IActionResult> Show(int id)
        {

            var categoryResponse = await _categoryService.GetAsync<APIResponse>(id);
            var category = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(categoryResponse.Result));

            var postsResponse = await _postService.GetAllAsync<APIResponse>();
            List<PostDTO> allPosts = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(postsResponse.Result));
            var categoryPosts = allPosts.Where(p => p.CategoryId == id).ToList();
            ViewBag.Category = category;
            ViewBag.Posts = categoryPosts;

            return View();
        }


    }
}
