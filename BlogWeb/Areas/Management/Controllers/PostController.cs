using AutoMapper;
using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Models.VM;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
        private readonly ILogger<PostController> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _token;
		public PostController(IPostService postService, IMapper mapper,
                              ICategoryService categoryService, IUserService userService,
                              ILogger<PostController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
            _categoryService = categoryService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

		}

        public async Task<IActionResult> Index()
        {
            //var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

			APIResponse response = await _postService.GetAllAsync<APIResponse>(_token);
            var posts = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(response.Result));
            _logger.LogInformation($"Token = {_token}");

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
                PostVM postCreateVM = new()
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
        public async Task<IActionResult> Create(PostVM postCreateVM)
        {
			// Prepare Category Select Items
			var categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
			var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));
			postCreateVM.Categories = categories.Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			var postDTO = _mapper.Map<PostCreateDTO>(postCreateVM.PostDTO);

			if (ModelState.IsValid)
            {
                if (postDTO != null)
                {
					// Using Session
					//var token = HttpContext.Session.GetString(SD.SessionToken);

					//Using Cookie
					//var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
					if (String.IsNullOrEmpty(_token))
                    {
                        ModelState.AddModelError(string.Empty, "Authentication token is missing");
                        return View(postCreateVM);
                    }

					var response = await _postService.CreateAsync<APIResponse>(postDTO, _token);
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

        [HttpGet]
		public async Task<IActionResult> Update(int id)
		{

			var postResponse = await _postService.GetAsync<APIResponse>(id,_token);
            if (postResponse != null && postResponse.IsSuccess == true)
            {
                var categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
                if (categoryResponse == null || categoryResponse.IsSuccess == false)
                {
                    TempData["error"] = "There's no categories!";
                    return View();
                }
                var postUpdateDTO = JsonConvert.DeserializeObject<PostUpdateDTO>(Convert.ToString(postResponse.Result));
                var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));

                PostUpdateVM postUpdateVM = new()
                {
                    Categories = categories.Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                    PostUpdateDTO = postUpdateDTO
				};
                return View(postUpdateVM);
            }
            else
            {
                TempData["error"] = "Something went wrong!";
                return RedirectToAction("Index");
            }

		}



        [HttpPost]
        public async Task<IActionResult> Update(PostUpdateVM postUpdateVM)
        {
			var categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
			var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));
			postUpdateVM.Categories = categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
			if (ModelState.IsValid)
            {
				var response = await _postService.UpdateAsync<APIResponse>(postUpdateVM.PostUpdateDTO, _token);

                if(response != null && response.IsSuccess == true)
                {
					TempData["success"] = "Post updated successfully";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					// Handle errors
					if (response.ErrorMessages != null && response.ErrorMessages.Any())
					{
						foreach (var error in response.ErrorMessages)
						{
							ModelState.AddModelError(string.Empty, error);
						}
					}
				}
			}

			TempData["error"] = "Something went wrong!";
			return View(postUpdateVM);


		}


	}
}
