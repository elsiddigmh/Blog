using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Relationships
        [Required]
		[ForeignKey("Post")]
		public int PostId { get; set; }
		[JsonIgnore]
		public Post Post { get; set; } // Navigation Property
        [Required]
		[ForeignKey("User")]
		public int UserId { get; set; }
		[JsonIgnore]
		public User User { get; set; } // Navigation Property
    }
}
