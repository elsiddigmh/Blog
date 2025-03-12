using BlogWeb.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogWeb.Models.VM
{
	public class PostVM
	{
		public PostDTO PostDTO { get; set; }
		public IEnumerable<SelectListItem> Categories { get; set; }
	}
}
