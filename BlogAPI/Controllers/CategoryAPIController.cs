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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync(includeProperties: ["Posts"]);

            if (categories == null)
            {
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


            return _response;
        }


        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"Invalid id {id} value");
                return BadRequest(_response);
            }
            bool isCategoryFound = await _categoryRepository.GetAsync(u => u.Id == id) != null;

            if (!isCategoryFound)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"Category with id {id} not found");
                return NotFound(_response);
            }

            var category = await _categoryRepository.GetAsync(u => u.Id == id, includeProperties: ["Posts"]);

            //Manuel Mapping
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
            return _response;
        }

        [HttpPost(Name = "CreateCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO createDTO)
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


        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateCategory([FromBody] CategoryUpdateDTO updateDTO, int id)
        {
            if (updateDTO == null || id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                ModelState.AddModelError("ErrorMessages", "Invalid or empty data");
                return BadRequest(_response);
            }
            var category = await _categoryRepository.GetAsync(u => u.Id == id);
            if (category == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                ModelState.AddModelError("ErrorMessages", "Category not found");
                return NotFound(_response);
            }
            category = new Category
            {
                Id = id,
                Name = updateDTO.Name,
                Slug = Helpers.Helpers.GenerateSlug(updateDTO.Name)
            };
            await _categoryRepository.UpdateAsync(category);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            _response.Result = category;

            return _response;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            if(id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"Invalid id {id} value");
                return BadRequest(_response);
            }

            var category = await _categoryRepository.GetAsync(u=>u.Id  == id);
            await _categoryRepository.RemoveAsync(category);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return _response;
        }
    }
}