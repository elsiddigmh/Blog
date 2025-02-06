using AutoMapper;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<UserDTO> list = new ();
            var response = await _userService.GetAllAsync<APIResponse>();
            if (response == null && response.IsSuccess == false)
            {
                TempData["error"] = "There's no users";
                return View();
            }
            list = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
            return View(list);
        }
    }
}
