using AutoMapper;
using BlogAPI.Models;
using BlogAPI.Models.Dto;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/PostAPI")]
    public class PostAPIController : Controller
    {
        private readonly IPostRepository _postRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        public PostAPIController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpGet(Name = "GetPosts")]
        public async Task<ActionResult<APIResponse>> GetPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            if (posts == null || posts.Count == 0)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("There's no posts");
                return NotFound(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = posts;
            return Ok(_response);

        }

        [HttpGet("{id:int}", Name = "GetPost")]
        public async Task<ActionResult<APIResponse>> GetPost(int id)
        {
            var post = await _postRepository.GetAsync(u => u.Id == id);
            if (post == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("There's no post whith this id");
                return NotFound(_response);
            }

            //Manuel Mapping
            //PostDTO postDTO = new PostDTO
            //{
            //    Id = id,
            //   ... = ... ect
            //}
            //
            PostDTO postDTO = _mapper.Map<PostDTO>(post);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = post;
            return Ok(_response);

        }

        [HttpPost(Name = "CreatePost")]
        public async Task<ActionResult<APIResponse>> CreatePost([FromBody] PostCreateDTO postDTO)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ModelState.ToString());
                return BadRequest(_response);
            }

            Post post = _mapper.Map<Post>(postDTO);
            post.Slug = Helpers.Helpers.GenerateSlug(post.Title);
            await _postRepository.CreateAsync(post);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = _mapper.Map<PostDTO>(post);

            return Ok(_response);

        }

        [HttpPut("{id:int}", Name = "UpdatePost")]
        public async Task<ActionResult<APIResponse>> UpdatePost(int id, [FromBody] PostUpdateDTO postDTO)
        {
            if (id == 0 || postDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Can't send empty data");
                return BadRequest(_response);
            }

            bool isFoundPost = await _postRepository.GetAsync(u => u.Id == id) != null;

            if (!isFoundPost)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Post not found");
                return BadRequest(_response);
            }

            Post post = _mapper.Map<Post>(postDTO);
            post.Slug = Helpers.Helpers.GenerateSlug(post.Title);
            await _postRepository.UpdateAsync(post);

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;

            return Ok(_response);

        }

        [HttpDelete("{id:int}", Name = "DeletePost")]
        public async Task<ActionResult<APIResponse>> DeletePost(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Invalid Id value");
                return BadRequest(_response);
            }

            Post post = await _postRepository.GetAsync(u => u.Id == id);
            if (post == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Post not found");
                return BadRequest(_response);
            }

            await _postRepository.RemoveAsync(post);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);


        }
    }
}
