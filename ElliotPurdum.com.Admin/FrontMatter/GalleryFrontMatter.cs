using TimPurdum.Dev.BlogGenerator.Admin.ContentTypes;
using YamlDotNet.Serialization;

namespace ElliotPurdum.Admin.FrontMatter;

public sealed class GalleryFrontMatter : IHasLastmodified
{
    [YamlMember(Alias = "layout")] public string Layout { get; set; } = "gallery";
    [YamlMember(Alias = "title")] public string Title { get; set; } = "";
    [YamlMember(Alias = "description")] public string? Description { get; set; }
    [YamlMember(Alias = "images")] public List<GalleryImageEntry> Images { get; set; } = [];
    [YamlMember(Alias = "lastmodified")] public string? Lastmodified { get; set; }
}

public sealed class GalleryImageEntry
{
    [YamlMember(Alias = "src")] public string Src { get; set; } = "";
    [YamlMember(Alias = "caption")] public string? Caption { get; set; }
}
