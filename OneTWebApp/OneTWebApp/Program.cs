using MudBlazor.Services;
using OneTWebApp.Components;
using OneTWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// HttpClient pour appeler les API internes

builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new Uri("http://localhost:5265/")
});
//> Added MudBlazor
builder.Services.AddMudServices();

//> Added Services
builder.Services.AddScoped<KubService>();

//> Added Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
}
else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(OneTWebApp.Client._Imports).Assembly);

app.MapControllers();

app.Run();