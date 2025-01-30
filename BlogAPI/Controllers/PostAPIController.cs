using BlogAPI.Models;
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

        public PostAPIController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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
            var post = await _postRepository.GetAsync(u=>u.Id == id);
            if (post == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("There's no post whith this id");
                return NotFound(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = post;
            return Ok(_response);

        }



    }
}
