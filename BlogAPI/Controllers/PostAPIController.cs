using AutoMapper;
using BlogAPI.Models;
using BlogAPI.Models.Dto;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogAPI.Controllers
{
	[ApiController]
	[Route("api/PostAPI")]
	public class PostAPIController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IPostRepository _postRepository;
		private readonly ICategoryRepository _categoryRepository;
		protected APIResponse _response;
		private readonly IMapper _mapper;
		public PostAPIController(IWebHostEnvironment webHostEnvironment, IPostRepository postRepository, ICategoryRepository categoryRepository, IMapper mapper)
		{
			_webHostEnvironment = webHostEnvironment;
			_postRepository = postRepository;
			_categoryRepository = categoryRepository;
			_mapper = mapper;
			_response = new APIResponse();
		}


		[HttpGet(Name = "GetPosts")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> GetPosts()
		{
			var posts = await _postRepository.GetAllAsync(includeProperties: ["Author", "Category"]);
			if (posts == null || posts.Count == 0)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("There's no posts");
				return NotFound(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			_response.Result = _mapper.Map<List<PostDTO>>(posts);
			return _response;

		}


		[HttpGet("{id:int}", Name = "GetPost")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> GetPost(int id)
		{
			var post = await _postRepository.GetAsync(u => u.Id == id, includeProperties: ["Author", "Category"]);
			if (post == null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add($"Post not found with Id {id} value");
				return NotFound(_response);
			}

			PostDTO postDTO = _mapper.Map<PostDTO>(post);

			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			_response.Result = postDTO;
			return _response;

		}

		[HttpPost(Name = "CreatePost")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Consumes("multipart/form-data")]
		public async Task<ActionResult<APIResponse>> CreatePost([FromForm] PostCreateDTO postDTO)
		{
			if (await _categoryRepository.GetAsync(u => u.Id == postDTO.CategoryId) == null)
			{
				ModelState.AddModelError("ErrorMessages", "Invalid category value!");
				return BadRequest(ModelState);
			}

			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages = errors;
				return BadRequest(_response);
			}

			Post post = _mapper.Map<Post>(postDTO);


			if (postDTO.File != null)
			{
                //string directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads\\Posts\\");
                string directoryPath = "Uploads\\Posts\\";
                string filePath = $"{directoryPath + Guid.NewGuid()}_{Path.GetFileName(postDTO.File.FileName)}";
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					postDTO.File.CopyTo(stream);
				}
				post.PhotoUrl = filePath;
			}
			else
			{
				post.PhotoUrl = string.Empty;
			}

			post.Slug = Helpers.Helpers.GenerateSlug(post.Title);
			await _postRepository.CreateAsync(post);
			_response.StatusCode = HttpStatusCode.Created;
			_response.IsSuccess = true;
			_response.Result = _mapper.Map<PostDTO>(post);

			return _response;

		}

		[HttpPut("{id:int}", Name = "UpdatePost")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Consumes("multipart/form-data")]
		public async Task<ActionResult<APIResponse>> UpdatePost(int id, [FromForm] PostUpdateDTO postDTO)
		{
			if (id <= 0 || postDTO == null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Can't send empty data");
				return BadRequest(_response);
			}

			// Get existing post
			var existingPost = await _postRepository.GetAsync(u => u.Id == id);
			if (existingPost == null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add($"Post with Id {id} not found ");
				return NotFound(_response);
			}

			Post post = _mapper.Map<Post>(postDTO);
						post.Slug = Helpers.Helpers.GenerateSlug(post.Title);

			if (postDTO.File != null)
			{
				// Delete old image if it exists
				if (!string.IsNullOrEmpty(existingPost.PhotoUrl) && System.IO.File.Exists(existingPost.PhotoUrl))
				{
					System.IO.File.Delete(existingPost.PhotoUrl);
				}

				// Save new image
				string directoryPath = "Uploads\\Posts\\";
				string filePath = $"{directoryPath + Guid.NewGuid()}_{Path.GetFileName(postDTO.File.FileName)}";
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					postDTO.File.CopyTo(stream);
				}
				post.PhotoUrl = filePath;
			}
			else
			{
				post.PhotoUrl = existingPost.PhotoUrl;
			}

			// Here fix problem of set PhotoUrl = null on update
			// Continue in clean article content from html tags in razor view
			await _postRepository.UpdateAsync(post);

			_response.StatusCode = HttpStatusCode.OK;
			_response.Result = _mapper.Map<PostDTO>(post);
			_response.IsSuccess = true;

			return Ok(_response);

		}

		[HttpDelete("{id:int}", Name = "DeletePost")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> DeletePost(int id)
		{
			if (id <= 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add($"Invalid id {id} value");
				return BadRequest(_response);
			}

			Post post = await _postRepository.GetAsync(u => u.Id == id);
			if (post == null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add($"Post with Id {id} not found");
				return BadRequest(_response);
			}

			if (!string.IsNullOrEmpty(post.PhotoUrl) && System.IO.File.Exists(post.PhotoUrl))
			{
				System.IO.File.Delete(post.PhotoUrl);
			}

			await _postRepository.RemoveAsync(post);
			_response.StatusCode = HttpStatusCode.NoContent;
			_response.IsSuccess = true;
			return _response;


		}
	}
}
