using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new Uri("http://localhost:5265/")
});

await builder.Build().RunAsync();