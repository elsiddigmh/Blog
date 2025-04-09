using BlogWeb.Models.Dto;

namespace BlogWeb.Models.VM
{
	public class DashboardViewModel
	{
		public List<UserDTO> Users { get; set; }
		public List<CategoryDTO> Categories { get; set; }
		public List<PostDTO> Posts { get; set; }
		public List<CommentDTO> Comments { get; set; }
	}
}
