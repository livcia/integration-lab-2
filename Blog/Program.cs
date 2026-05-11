using Blog.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<Blog.Services.PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Automatyczne aplikowanie migracji (tworzenie tabel) przed startem aplikacji
using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    using (var context = new Blog.Data.PostsContext(config))
    {
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            context.Users.Add(new Blog.Data.User
            {
                Nickname = "Administrator",
                Email = "admin@blog.local",
                Password = "haslo",
                RegisterDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            context.SaveChanges();
        }
    }
}

app.Run();
