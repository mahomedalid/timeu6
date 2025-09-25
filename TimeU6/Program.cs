using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TimeU6;
using TimeU6.Models;
using TimeU6.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register TimeU6 services
builder.Services.AddScoped<MatchState>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMatchService, MatchService>();

// Register local storage services
builder.Services.AddScoped<ILocalStorageHelper, LocalStorageHelper>();
builder.Services.AddScoped<IMatchStateStorage, MatchStateStorage>();
builder.Services.AddScoped<MatchStateInitializer>();

await builder.Build().RunAsync();
