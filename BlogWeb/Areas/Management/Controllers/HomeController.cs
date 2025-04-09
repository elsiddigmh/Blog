using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Models.VM;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWeb.Areas.Admin.Controllers
{
    [Area("Management")]
    public class HomeController : Controller
    {
		private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        //private readonly ICommentService _commentService;

		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly string _token;
		public HomeController(ICategoryService categoryService, IPostService postService,
                              IUserService userService, IHttpContextAccessor httpContextAccessor)
		{
			_categoryService = categoryService;
			_postService = postService;
			_userService = userService;
			//_commentService = commentService;

			_httpContextAccessor = httpContextAccessor;
			_token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

		}

		public async Task<IActionResult> Index()
        {
			var userResponse = await _userService.GetAllAsync<APIResponse>();
			var users = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(userResponse.Result));

			var categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
            var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));

            var postResponse = await _postService.GetAllAsync<APIResponse>(_token);
            var posts = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(postResponse.Result));

			//var commentResponse = await _commentService.GetAllAsync<APIResponse>();
			//var comments = JsonConvert.DeserializeObject<List<CommentDTO>>(Convert.ToString(commentResponse.Result));

			var dashboardVM = new DashboardViewModel
			{
				Users = users,
				Categories = categories,
				Posts = posts,
				//Comments = comments
			};


            return View(dashboardVM);
        }
    }
}
