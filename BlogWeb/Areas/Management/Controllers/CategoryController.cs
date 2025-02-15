using AutoMapper;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWeb.Areas.Management.Controllers
{
    [Area("Management")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            APIResponse response = await _categoryService.GetAllAsync<APIResponse>();
            var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO categoryDTO)
        {
            if (ModelState.IsValid) {
                if (categoryDTO != null) { 
                    var response = await _categoryService.CreateAsync<APIResponse>(categoryDTO);
                    if (response != null && response.IsSuccess == true) {
                        TempData["success"] = "Category created successfully";
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
            }
			TempData["error"] = "Something went wrong!";
			return View(categoryDTO);
        }



    }
}
