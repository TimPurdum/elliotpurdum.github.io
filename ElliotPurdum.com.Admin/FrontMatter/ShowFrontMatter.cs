using TimPurdum.Dev.BlogGenerator.Admin.ContentTypes;
using YamlDotNet.Serialization;

namespace ElliotPurdum.Admin.FrontMatter;

public sealed class ShowFrontMatter : IHasLastmodified
{
    [YamlMember(Alias = "layout")] public string Layout { get; set; } = "show";
    [YamlMember(Alias = "title")] public string Title { get; set; } = "";
    [YamlMember(Alias = "time")] public string? Time { get; set; }
    [YamlMember(Alias = "venue")] public string? Venue { get; set; }
    [YamlMember(Alias = "city")] public string? City { get; set; }
    [YamlMember(Alias = "ticketUrl")] public string? TicketUrl { get; set; }
    [YamlMember(Alias = "program")] public string? Program { get; set; }
    [YamlMember(Alias = "role")] public string? Role { get; set; }
    [YamlMember(Alias = "description")] public string? Description { get; set; }
    [YamlMember(Alias = "lastmodified")] public string? Lastmodified { get; set; }
}
