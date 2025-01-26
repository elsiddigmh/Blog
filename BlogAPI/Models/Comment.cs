namespace BlogAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }


        // Relationships
        public int PostId { get; set; }
        public Post Post { get; set; } // Navigation Property

        public int UserId { get; set; }
        public User User { get; set; } // Navigation Property
    }
}
