using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Models.VM;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWeb.ViewComponents
{
    public class LatestArticlesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _token;
        private readonly string _baseUrl;
        public LatestArticlesViewComponent(IConfiguration configuration ,ICategoryService categoryService, IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
            _baseUrl = configuration.GetValue<string>("ServiceUrls:BlogAPI");
            _categoryService = categoryService;
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
            _token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
            var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));

            var postResponse = await _postService.GetAllAsync<APIResponse>();
            var posts = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(postResponse.Result));

            var latestOne = posts.OrderByDescending(p => p.Id).FirstOrDefault();
            var latestPosts = posts.OrderByDescending(p => p.Id).Take(5).ToList();

            ViewData["latestOne"] = latestOne;
            ViewData["latestPosts"] = latestPosts;
            ViewData["baseUrl"] = _baseUrl;

            return View();
        }
    }
}
