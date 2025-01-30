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
            foreach (var category in categories)
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


        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<APIResponse>> GetGategory(int id)
        {
            if (id == null || id == 0) { 
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Invalid id value");
                return BadRequest(_response);
            }
            var category = await _categoryRepository.GetAsync(u=>u.Id == id);
            CategoryDTO categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Posts = category.Posts,
            };
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = categoryDTO;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost(Name = "CreateCategory")]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody]CategoryCreateDTO createDTO)
        {
            bool isNameFound = await _categoryRepository.GetAsync(u => u.Name == createDTO.Name) != null;
            if (isNameFound)
            {
                ModelState.AddModelError("ErrorMessages", "Category name already exists!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = new Category
            {
                Name = createDTO.Name,
                Slug = Helpers.Helpers.GenerateSlug(createDTO.Name)
            };

            await _categoryRepository.CreateAsync(category);
            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = category;
            _response.IsSuccess = true;
            return CreatedAtRoute("GetCategory", new { id = category.Id }, _response);

        }



    }
}
