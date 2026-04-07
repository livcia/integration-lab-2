using Blog.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blog.Components.Pages
{
    public partial class Admin
    {
        [Inject] private PostService postService { get; set; } = null!;
        [Inject] public IJSRuntime JS { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;
        private DotNetObjectReference<Admin>? componentRef;
        private int? activeMenuId = null;
        private string ActivityLevel = "Niska";
        private ElementReference mainContainer;
        private List<Data.Post> posts = new();

        protected override Task OnInitializedAsync()
        {
            postService.ClearCache();
            posts = postService.ReadPostsAfterUpdate();
            ActivityLevel = postService.GetActivityLevel();
            return Task.CompletedTask;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                componentRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("setupClickOutside", mainContainer, componentRef);
            }
        }

        [JSInvokable]
        public void CloseMenu()
        {
            activeMenuId = null;
            StateHasChanged();
        }

        private void ToggleMenu(int postId)
        {
            activeMenuId = activeMenuId == postId ? null : postId;
        }

        private void EditPost(int postId)
        {
            Navigation.NavigateTo($"/admin/post/{postId}");
        }

        private void TogglePublish(int postId)
        {
            activeMenuId = null;
            postService.TogglePublish(postId);            
            posts = postService.ReadPostsAfterUpdate();
            ActivityLevel = postService.GetActivityLevel();

        }

        private void DeletePost(int postId)
        {
            activeMenuId = null;
            postService.DeletePost(postId);
            posts = postService.ReadPostsAfterUpdate();
            ActivityLevel = postService.GetActivityLevel();
        }

    }
}
