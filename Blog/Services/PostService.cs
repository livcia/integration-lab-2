using Blog.Data;
using Microsoft.EntityFrameworkCore;
namespace Blog.Services
{
    public partial class PostService
    {
        private PostsContext? _context;
        private List<Post>? _cachedPosts;

        public PostService()
        {
            _context = new PostsContext(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
        }

        public void ResetContext(){
            _context = new PostsContext(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
        }
         //CRUD:
        public void CreatePost(Post newPost)
        {
            _context?.Posts.Add(newPost);
            _context?.SaveChanges();
        }

        public List<Post> ReadPosts()
        {
            if (_context == null)
            {
                return new List<Post>();
            }
            if (_cachedPosts == null)
            {
                _cachedPosts = _context.Posts.Include(p => p.User).ToList();
            }
            return _cachedPosts;
        }

        public List<User> GetUsers()
        {
            if (_context == null)
            {
                return new List<User>();
            }
            return _context.Users.ToList();
        }

        public List<Post> ReadPostsAfterUpdate()
        {
            ResetContext();
            if (_context == null)
            {
                return new List<Post>();
            }
            _cachedPosts = _context.Posts.Include(p => p.User).ToList();
            return _cachedPosts;
        }
        public string GetActivityLevel()
        {
            var posts = ReadPosts();
            var lastSevenDays = DateTime.Now.AddDays(-7);

            var recentPostsCount = posts.Where(p => DateTime.TryParse(p.Created_At, out var createdDate) && createdDate >= lastSevenDays).Count();
            if (recentPostsCount >= 2) return "Wysoka";
            if (recentPostsCount == 1) return "Średnia";
            return "Niska";
        }
        public Post GetPostById(int id)
        {
            var post = _context?.Posts.Include(p => p.User).FirstOrDefault(p => p.Id == id);
            return post!;
        }
        public void TogglePublish(int postId)
        {
            var post = _context?.Posts.Find(postId);
            if (post != null)
            {
                post.is_published = !post.is_published;
                if (post.is_published) post.Published_At = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                else post.Published_At = null;
                _context?.SaveChanges();
            }
        }

        public void UpdatePost(Post updatedPost)
        {
            _context?.Posts.Update(updatedPost);
            _context?.SaveChanges();
            ClearCache();
        }

        public void DeletePost(int postId)
        {
            var post = _context?.Posts.Find(postId);
            if (post != null)
            {
                _context?.Posts.Remove(post);
                _context?.SaveChanges();
            }
        }

        public void ClearCache()
        {
            ResetContext();
            _cachedPosts = null;
        }

        public void changePublishStatus(int postId, bool newStatus)
        {
            var post = _context?.Posts.Find(postId);
            if (post != null)
            {
                post.is_published = newStatus;
                if (newStatus) post.Published_At = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                else post.Published_At = null;
                _context?.SaveChanges();
            }
        }
    }
    
   
}