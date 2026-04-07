using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Components;

namespace Blog.Components.Pages
{
    public partial class PostDetail
    {
        public Data.Post? CurrentPost { get; set; }
        private List<Data.Post> posts = new();
        [Parameter] public int Id { get; set; }
        [Inject] private PostService postService { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            posts = postService.ReadPostsAfterUpdate();
            CurrentPost = posts.FirstOrDefault(p => p.Id == Id);
            if (CurrentPost == null)
            {
                NavigationManager.NavigateTo("/not-found");
        }
            await Task.CompletedTask;
        }
    }
}