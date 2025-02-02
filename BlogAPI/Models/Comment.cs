using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; } // Navigation Property
        [Required]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; } // Navigation Property
    }
}
