﻿using AutoMapper;
using BlogAPI.Models;
using BlogAPI.Models.Dto;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogAPI.Controllers
{

    [Route("api/CommentAPI")]
    [ApiController]
    public class CommentAPIController : Controller
    {
        protected APIResponse _response;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public CommentAPIController(ICommentRepository commentRepository, IMapper mapper)
        {
            _response = new APIResponse();
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetComments()
        {
            var comments = await _commentRepository.GetAllAsync(includeProperties: ["User", "Post"]);

            if (comments == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("There's no comments");
                return BadRequest(_response);
            }
            List<CommentDTO> commentsDTO = _mapper.Map<List<CommentDTO>>(comments);
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = commentsDTO;
            _response.IsSuccess = true;


            return _response;
        }


        [HttpGet("{id:int}", Name = "GetComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetComment(int id)
        {
            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"Invalid id {id} value");
                return BadRequest(_response);
            }

            bool isCommentFound = await _commentRepository.GetAsync(u=>u.Id == id) != null;

            if (!isCommentFound)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"Comment with id {id} not found");
                return NotFound(_response);
            }

            var comment = await _commentRepository.GetAsync(u => u.Id == id, includeProperties: ["User", "Post"]);
            CommentDTO CommentDTO = _mapper.Map<CommentDTO>(comment);
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = CommentDTO;
            _response.IsSuccess = true;
            return _response;
        }


        [HttpPost(Name = "CreateComment")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateComment([FromBody] CommentCreateDTO createDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Comment comment = _mapper.Map<Comment>(createDTO);

            await _commentRepository.CreateAsync(comment);
            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = comment;
            _response.IsSuccess = true;
            return CreatedAtRoute("GetComment", new { id = comment.Id }, _response);

        }


        [HttpPut("{id:int}", Name = "UpdateComment")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateComment([FromBody] CommentUpdateDTO updateDTO, int id)
        {
            if (updateDTO == null || id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                ModelState.AddModelError("ErrorMessages", "Invalid or empty data");
                return BadRequest(_response);
            }

            bool isCommentFound = await _commentRepository.GetAsync(u => u.Id == id) != null;
            if (!isCommentFound)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                ModelState.AddModelError("ErrorMessages", "Comment not found");
                return NotFound(_response);
            }
            Comment comment = _mapper.Map<Comment>(updateDTO);
            CommentDTO commentDTO = _mapper.Map<CommentDTO>(comment);
            await _commentRepository.UpdateAsync(comment);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            _response.Result = commentDTO;
            return _response;
        }


        [HttpDelete("{id:int}", Name = "DeleteComment")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteComment(int id)
        {
            if(id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"Invalid id {id} value");
                return BadRequest(_response);
            }

            var comment = await _commentRepository.GetAsync(u=>u.Id  == id);
            await _commentRepository.RemoveAsync(comment);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return _response;


        }
    }
}