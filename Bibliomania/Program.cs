using Bibliomania.Components;
using Bibliomania.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddHttpClient<OpenLibraryService>(client =>
{
	client.BaseAddress = new Uri("https://openlibrary.org/");
	client.DefaultRequestHeaders.UserAgent.ParseAdd("Bibliomania/1.0");
});

builder.Services.AddHttpClient<GoogleBooksService>(client =>
{
	client.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
});

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

app.Run();
