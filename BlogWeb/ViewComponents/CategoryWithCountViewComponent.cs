using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Models.VM;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWeb.ViewComponents
{
    public class CategoryWithCountViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _token;
        public CategoryWithCountViewComponent(ICategoryService categoryService, IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
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

            var model = categories.Select(cat => new CategoryWithCountVM
            {
                Id = cat.Id,
                Name = cat.Name,
                PostCount = posts.Count(p => p.CategoryId == cat.Id)
            }).ToList();

            return View(model);
        }
    }
}
