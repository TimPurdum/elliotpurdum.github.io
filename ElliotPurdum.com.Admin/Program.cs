using ElliotPurdum.Admin;
using ElliotPurdum.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// One HttpClient pointed at the GitHub REST API. AuthService stamps the bearer token per request.
builder.Services.AddSingleton(_ => new HttpClient { BaseAddress = new Uri("https://api.github.com/") });

// Repo coordinates. Hardcoded for the elliotpurdum.com site for now; surface to config later if needed.
builder.Services.AddSingleton(new GitHubRepoConfig(Owner: "TimPurdum", Repo: "elliotpurdum.github.io"));

builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<GitHubApiService>();
builder.Services.AddSingleton<DeployStatusService>();

await builder.Build().RunAsync();
