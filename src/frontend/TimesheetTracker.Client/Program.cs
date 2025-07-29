using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TimesheetTracker.Client;
using TimesheetTracker.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app"); // Use standard Blazor registration
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for API calls
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://localhost:5000/") // Backend API URL
});

// Register services
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var host = builder.Build();

// Initialize authentication
var authService = host.Services.GetRequiredService<IAuthenticationService>();
await authService.InitializeAsync();

await host.RunAsync();
