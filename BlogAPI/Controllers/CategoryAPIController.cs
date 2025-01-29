using BlogAPI.Models;
using BlogAPI.Models.Dto;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("There's no categories");
                return BadRequest(_response);
            }
            List<CategoryDTO> categoriesDTO = new List<CategoryDTO>();
            foreach(var category in categories)
            {
                var categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Slug = category.Slug,
                    Posts = category.Posts,
                };

                categoriesDTO.Add(categoryDTO);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = categoriesDTO;
            _response.IsSuccess = true;


            return Ok(_response);
        }
    }
}
