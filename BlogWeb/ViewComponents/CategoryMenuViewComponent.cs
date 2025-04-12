using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;

namespace BlogWeb.ViewComponents
{
	public class CategoryMenuViewComponent : ViewComponent
	{
		private readonly ICategoryService _categoryService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public CategoryMenuViewComponent(ICategoryService categoryService, IHttpContextAccessor httpContextAccessor)
		{
			_categoryService = categoryService;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var response = await _categoryService.GetAllAsync<APIResponse>();
			var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
			return View(categories);
		}
	}
}
