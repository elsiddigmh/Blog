using AutoMapper;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Models.VM;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace BlogWeb.Areas.Management.Controllers
{
    [Area("Management")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public PostController(IPostService postService, IMapper mapper, ICategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            APIResponse response = await _postService.GetAllAsync<APIResponse>();
            var posts = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(response.Result));
            return View(posts);
        }

        public async Task<IActionResult> Create()
        {
            var response = await _categoryService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true) {
                var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
                PostCreateVM postCreateVM = new()
                {
                    Categories = categories.Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                };
				return View(postCreateVM);
			}
			else
            {
				TempData["error"] = "There's no categories!";
				return View();
			}

        }
    }
}
