using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Data
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Created_At { get; set; }
        public string? Published_At { get; set; }
        public string? Photo { get; set; }
        public bool is_published { get; set; } = false;

        public int author { get; set; }

        [ForeignKey(nameof(author))]
        public User? User { get; set; }
    }
}