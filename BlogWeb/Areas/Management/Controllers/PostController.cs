using AutoMapper;
using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Models.VM;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BlogWeb.Areas.Management.Controllers
{
    [Area("Management")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public PostController(IPostService postService, IMapper mapper, ICategoryService categoryService, IUserService userService)
        {
            _postService = postService;
            _categoryService = categoryService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            APIResponse response = await _postService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var posts = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(response.Result));
            return View(posts);
        }

        public async Task<IActionResult> Create()
        {
			var userEmailClaim = User.FindFirst(ClaimTypes.Email).Value;
            var userResponse = await _userService.GetAllAsync<APIResponse>();
            var users = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(userResponse.Result));
            var author = users.FirstOrDefault(u=>u.Email == userEmailClaim);

			ViewBag.AuthorId = author.Id;
			var response = await _categoryService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
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


        [HttpPost]
        public async Task<IActionResult> Create(PostCreateDTO postDTO)
        {
			// Prepare Category Select Items
			var categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
			var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));
			PostCreateVM postCreateVM = new()
			{
                PostDTO = _mapper.Map<PostDTO>(postDTO),
				Categories = categories.Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
			};

			if (ModelState.IsValid)
            {
                if (postDTO != null)
                {
                    var token = HttpContext.Session.GetString(SD.SessionToken);
                    if (String.IsNullOrEmpty(token))
                    {
                        ModelState.AddModelError(string.Empty, "Authentication token is missing");
                        return View(postCreateVM);
                    }

					var response = await _postService.CreateAsync<APIResponse>(postDTO, token);
                    if (response != null && response.IsSuccess == true)
                    {
                        TempData["success"] = "Post created successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Handle errors
                        if (response?.ErrorMessages != null)
                        {
                            foreach (var error in response.ErrorMessages)
                            {
                                ModelState.AddModelError(string.Empty, error);
                            }
                        }
                    }
                }

            }
            TempData["error"] = "Something went wrong!";
            return View(postCreateVM);

        }


    }
}
