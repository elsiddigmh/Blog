using AutoMapper;
using BlogUtility;
using BlogWeb.Models;
using BlogWeb.Models.Dto;
using BlogWeb.Models.VM;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;

namespace BlogWeb.Areas.Admin.Controllers
{
    [Area("Management")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


		public async Task<IActionResult> Create()
        {
			UserCreateVM userCreateVM = new UserCreateVM
			{
				Roles = new List<SelectListItem>
				{
					new SelectListItem {Text = Roles.Admin, Value = Roles.Admin.ToLower()},
					new SelectListItem {Text = Roles.Author, Value = Roles.Author.ToLower()},
				}
			};
			return View(userCreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDTO userDTO)
        {
            if (ModelState.IsValid)
            {

				var response = await _userService.CreateAsync<APIResponse>(userDTO);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "User created successfully";
					return RedirectToAction(nameof(Index));
				}
			}

			TempData["error"] = "Something went wrong!";
			return View(userDTO);


		}

		public async Task<IActionResult> Index()
        {
            List<UserDTO> list = new();
            var response = await _userService.GetAllAsync<APIResponse>();
            if (response == null && response.IsSuccess == false)
            {
                return View();
            }
            list = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
            return View(list);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.GetAsync<APIResponse>(id);
            if (response == null && response.IsSuccess == false)
            {
                NotFound();
            }
            UserDTO userDto = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result));
            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDTO userDTO)
        {
            var response = await _userService.DeleteAsync<APIResponse>(userDTO.Id);

            if (response == null && response.IsSuccess == false)
            {
                TempData["error"] = "Something went wrong!";
                return View(userDTO);
            }
            TempData["success"] = "User deleted successfully";
            return RedirectToAction(nameof(Index));
        }



    }
}
