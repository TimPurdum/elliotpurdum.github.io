using YamlDotNet.Serialization;

namespace ElliotPurdum.Admin.Services;

/// <summary>
/// Each content type has its own frontmatter shape. These are deserialized from / serialized to
/// the YAML block at the top of the markdown file.
///
/// Property order in YAML output follows the order of properties here, so keep human-friendly
/// fields (Title, Layout) first and machine-managed fields (Lastmodified) last.
///
/// <see cref="YamlMemberAttribute"/> is used to lowercase the keys, matching the convention BlogGenerator
/// already enforces in the rest of the site's content.
/// </summary>
public sealed class PostFrontMatter
{
    [YamlMember(Alias = "layout")] public string Layout { get; set; } = "post";
    [YamlMember(Alias = "title")] public string Title { get; set; } = "";
    [YamlMember(Alias = "subtitle")] public string? Subtitle { get; set; }
    [YamlMember(Alias = "description")] public string? Description { get; set; }
    [YamlMember(Alias = "lastmodified")] public string? Lastmodified { get; set; }
}

public sealed class MusicFrontMatter
{
    [YamlMember(Alias = "layout")] public string Layout { get; set; } = "music";
    [YamlMember(Alias = "title")] public string Title { get; set; } = "";
    /// <summary>"performance" | "recording" | "composition".</summary>
    [YamlMember(Alias = "type")] public string Type { get; set; } = "performance";
    [YamlMember(Alias = "ensemble")] public string? Ensemble { get; set; }
    /// <summary>"soloist" | "principal" | "section" | "conductor".</summary>
    [YamlMember(Alias = "role")] public string? Role { get; set; }
    [YamlMember(Alias = "venue")] public string? Venue { get; set; }
    [YamlMember(Alias = "embedUrl")] public string? EmbedUrl { get; set; }
    [YamlMember(Alias = "coverImage")] public string? CoverImage { get; set; }
    [YamlMember(Alias = "description")] public string? Description { get; set; }
    [YamlMember(Alias = "lastmodified")] public string? Lastmodified { get; set; }
}

public sealed class ShowFrontMatter
{
    [YamlMember(Alias = "layout")] public string Layout { get; set; } = "show";
    [YamlMember(Alias = "title")] public string Title { get; set; } = "";
    /// <summary>Free-form (e.g. "7:30 PM"). Date lives in the filename.</summary>
    [YamlMember(Alias = "time")] public string? Time { get; set; }
    [YamlMember(Alias = "venue")] public string? Venue { get; set; }
    [YamlMember(Alias = "city")] public string? City { get; set; }
    [YamlMember(Alias = "ticketUrl")] public string? TicketUrl { get; set; }
    [YamlMember(Alias = "program")] public string? Program { get; set; }
    [YamlMember(Alias = "role")] public string? Role { get; set; }
    [YamlMember(Alias = "description")] public string? Description { get; set; }
    [YamlMember(Alias = "lastmodified")] public string? Lastmodified { get; set; }
}

public sealed class GalleryFrontMatter
{
    [YamlMember(Alias = "layout")] public string Layout { get; set; } = "gallery";
    [YamlMember(Alias = "title")] public string Title { get; set; } = "";
    [YamlMember(Alias = "description")] public string? Description { get; set; }
    [YamlMember(Alias = "images")] public List<GalleryImageEntry> Images { get; set; } = new();
    [YamlMember(Alias = "lastmodified")] public string? Lastmodified { get; set; }
}

public sealed class GalleryImageEntry
{
    [YamlMember(Alias = "src")] public string Src { get; set; } = "";
    [YamlMember(Alias = "caption")] public string Caption { get; set; } = "";
}

/// <summary>
/// Static pages (about, index, etc.) — markdown files under <c>Content/Pages/</c> with no date prefix.
/// <see cref="Layout"/> chooses the rendering template — <c>page</c> for vanilla content, <c>home</c> for
/// the homepage hero + dynamic sections.
/// </summary>
public sealed class PageFrontMatter
{
    [YamlMember(Alias = "layout")] public string Layout { get; set; } = "page";
    [YamlMember(Alias = "title")] public string Title { get; set; } = "";
    [YamlMember(Alias = "subtitle")] public string? Subtitle { get; set; }
    [YamlMember(Alias = "description")] public string? Description { get; set; }
    [YamlMember(Alias = "lastmodified")] public string? Lastmodified { get; set; }
}
