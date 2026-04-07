
using Blog.Services;
using Blog.Data;
using Microsoft.AspNetCore.Components;
using Ganss.Xss;

namespace Blog.Components.Pages
{
    public partial class EditPost
    {
        [Parameter] public int? Id { get; set; }
        private List<Data.User> usersList = new();
        [Inject] private PostService PostService { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;
        private string PageTitle => Id.HasValue ? "Edit Post" : "Create New Post";
        Data.Post workingPost = null!;
        private bool isPreviewMode = false;

        protected override Task OnInitializedAsync()
        {
            usersList = PostService.GetUsers();
            if (Id.HasValue)
            {
                workingPost = PostService.GetPostById(Id.Value);
            }
            else
            {
                var defaultUser = usersList.FirstOrDefault();
                workingPost = new Data.Post
                {
                    Title = "",
                    Content = "",
                    Created_At = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Published_At = null,
                    Photo = null,
                    is_published = false,
                    author = defaultUser?.Id ?? 0,
                    User = defaultUser
                };
            }
            return Task.CompletedTask;
        }


        private void SwitchTab(bool toEditTab)
        {
            isPreviewMode = !toEditTab;
        }

        private void OnAuthorChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int authorId))
            {
                workingPost.author = authorId;
                workingPost.User = usersList.FirstOrDefault(u => u.Id == authorId);
            }
        }

        private void OnPublishedChanged(ChangeEventArgs e)
        {
            workingPost.is_published = (bool)e.Value!;
            if (workingPost.is_published)
            {
                workingPost.Published_At = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                workingPost.Published_At = null;
            }
        }

        private void SavePost()
        {
            if (Id.HasValue)
            {
                PostService.UpdatePost(workingPost);
            }
            else
            {
                PostService.CreatePost(workingPost);
            }
            Navigation.NavigateTo("/admin");
        }

        private void Cancel()
        {
           PostService.ClearCache();
           Navigation.NavigateTo("/admin");
        }

        private string SanitizeHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;
            
            var sanitizer = new HtmlSanitizer();
            return sanitizer.Sanitize(html);
        }
    }
}