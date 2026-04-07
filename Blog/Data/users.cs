namespace Blog.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? RegisterDate { get; set; }
        public ICollection<Post>? Posts { get; set; }
    }
}