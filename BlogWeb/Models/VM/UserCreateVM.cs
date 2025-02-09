using BlogUtility;
using BlogWeb.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogWeb.Models.VM
{
	public class UserCreateVM
	{
		public UserDTO UserDTO { get; set; }
		public IEnumerable<SelectListItem> Roles { get; set; }

    }
}
