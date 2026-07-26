using ElliotPurdum.Admin.Components;
using ElliotPurdum.Admin.FrontMatter;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TimPurdum.Dev.BlogGenerator.Admin;
using TimPurdum.Dev.BlogGenerator.Admin.ContentTypes;
using TimPurdum.Dev.BlogGenerator.Admin.Services;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<TimPurdum.Dev.BlogGenerator.Admin.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlogAdmin(opts =>
{
    opts.Repo = new GitHubRepoConfig(Owner: "TimPurdum", Repo: "elliotpurdum.github.io");
    opts.PatStorageKey = "elliotpurdum.admin.pat";
    opts.SiteName = "Elliot Purdum";
    opts.ImagesRoot = "ElliotPurdum.com.Source/wwwroot";
    opts.ImageFolders = ["images/music", "images/gallery", "images"];
    opts.PublicImageUrlPrefix = "";

    opts.ConfigurePost(p =>
    {
        p.ContentPath = "ElliotPurdum.com.Source/Content/Posts";
    });

    opts.RemoveDefaultPage();

    opts.AddContentType<MusicFrontMatter, MusicEditorForm>(
        slug: "music",
        displayName: "Music",
        contentPath: "ElliotPurdum.com.Source/Content/Music",
        namePattern: ContentNamePattern.Dated,
        singularNoun: "music entry",
        dashboardHint: "Portfolio entries: performances, recordings, compositions",
        order: 20,
        urlStem: "music");

    opts.AddContentType<ShowFrontMatter, ShowEditorForm>(
        slug: "shows",
        displayName: "Shows",
        contentPath: "ElliotPurdum.com.Source/Content/Shows",
        namePattern: ContentNamePattern.Dated,
        singularNoun: "show",
        dashboardHint: "Concert and event listings",
        order: 30,
        urlStem: "show");

    opts.AddContentType<GalleryFrontMatter, GalleryEditorForm>(
        slug: "gallery",
        displayName: "Gallery",
        contentPath: "ElliotPurdum.com.Source/Content/Gallery",
        namePattern: ContentNamePattern.YearMonth,
        singularNoun: "gallery",
        dashboardHint: "Photo gallery collections",
        order: 40,
        urlStem: "gallery");
});

await builder.Build().RunAsync();
