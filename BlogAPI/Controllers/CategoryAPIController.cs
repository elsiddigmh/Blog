using BlogAPI.Models;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{

    [Route("api/CategoryAPI")]
    [ApiController]
    public class CategoryAPIController : Controller
    {
        protected APIResponse _response;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryAPIController(ICategoryRepository categoryRepository)
        {
            _response = new APIResponse();
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            if (categories == null) {
                return BadRequest();
            }

            return Ok(categories);
        }
    }
}
