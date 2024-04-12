using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using NepaliRiddle;
using NepaliRiddle.Models;
using NepaliRiddle.Pages;
using NepaliRiddle.States;
using System.Net.Http.Json;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<DataHolder>();
builder.Services.AddScoped<GameStateManager>();
builder.Services.AddScoped<CountDownState>();
builder.Services.AddMudServices();
await builder.Build().RunAsync();

