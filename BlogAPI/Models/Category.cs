namespace BlogAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        // Relationships
        public ICollection<Post> Posts { get; set; } // One-to-Many
    }
}
