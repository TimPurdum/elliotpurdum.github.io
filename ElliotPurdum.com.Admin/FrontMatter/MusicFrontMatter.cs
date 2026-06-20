using TimPurdum.Dev.BlogGenerator.Admin.ContentTypes;
using YamlDotNet.Serialization;

namespace ElliotPurdum.Admin.FrontMatter;

public sealed class MusicFrontMatter : IHasLastmodified
{
    [YamlMember(Alias = "layout")] public string Layout { get; set; } = "music";
    [YamlMember(Alias = "title")] public string Title { get; set; } = "";
    [YamlMember(Alias = "type")] public string Type { get; set; } = "performance";
    [YamlMember(Alias = "ensemble")] public string? Ensemble { get; set; }
    [YamlMember(Alias = "role")] public string? Role { get; set; }
    [YamlMember(Alias = "venue")] public string? Venue { get; set; }
    [YamlMember(Alias = "embedUrl")] public string? EmbedUrl { get; set; }
    [YamlMember(Alias = "coverImage")] public string? CoverImage { get; set; }
    [YamlMember(Alias = "description")] public string? Description { get; set; }
    [YamlMember(Alias = "lastmodified")] public string? Lastmodified { get; set; }
}
