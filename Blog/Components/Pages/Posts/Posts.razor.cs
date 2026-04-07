using Blog.Services;
using Microsoft.AspNetCore.Components;

namespace Blog.Components.Pages
{
    public partial class Posts
    {
        [Inject] private PostService PostService { get; set; } = null!;
        private List<Data.Post> posts = new();

        protected override Task OnInitializedAsync()
        {
            posts = PostService.ReadPosts();
            return Task.CompletedTask;
        }
        
    }
}