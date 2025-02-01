using AutoMapper;
using BlogAPI.Models;
using BlogAPI.Models.Dto.User;
using BlogAPI.Repository;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/UserAPI")]
    public class UserAPIController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public UserAPIController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public async Task <ActionResult<APIResponse>> GetUser(int id)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Invalid user Id value");
                return NotFound(_response);
            }
            UserDTO userDTO = _mapper.Map<UserDTO>(user);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = userDTO;
            return Ok(_response);
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null || users.Count == 0)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("There's no users");
                return NotFound(_response);
            }
            List<UserDTO> usersDTO = _mapper.Map<List<UserDTO>>(users);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = usersDTO;
            return Ok(_response);
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody]UserCreateDTO userDTO)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ModelState.ToString());
                return BadRequest(_response);
            }

            bool isUsernameFound = await _userRepository.GetAsync(u=>u.UserName == userDTO.UserName) != null;
            if (isUsernameFound)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists!");
                return BadRequest(_response);
            }

            User user = _mapper.Map<User>(userDTO);
            await _userRepository.CreateAsync(user);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = _mapper.Map<UserDTO>(user);

            return Ok(_response);
        }

        [HttpPut("{id:int}", Name = "UpdateUser")]
        public async Task<ActionResult<APIResponse>> UpdateUser([FromBody] UserUpdateDTO userDTO, int id)
        {
            if (id == 0 || userDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Can't send empty data");
                return BadRequest(_response);
            }

            bool isFoundUser = await _userRepository.GetAsync(u => u.Id == id) != null;

            if (!isFoundUser)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User not found");
                return BadRequest(_response);
            }

            User user = _mapper.Map<User>(userDTO);
            await _userRepository.UpdateAsync(user);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;

            return Ok(_response);
        }


        [HttpDelete("{id:int}", Name = "DeleteUser")]
        public async Task<ActionResult<APIResponse>> DeleteUser(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Invalid Id value");
                return BadRequest(_response);
            }

            User user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User not found");
                return BadRequest(_response);
            }

            await _userRepository.RemoveAsync(user);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }

    }
}
