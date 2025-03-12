using BlogWeb.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogWeb.Models.VM
{
	public class PostUpdateVM
	{
		public PostUpdateDTO PostUpdateDTO { get; set; }
		public IEnumerable<SelectListItem> Categories { get; set; }
	}
}
