using BlogWeb.Models.Dto;

namespace BlogWeb.Models.VM
{
    public class PostPaginationVM
    {
        public List<PostDTO> Posts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
