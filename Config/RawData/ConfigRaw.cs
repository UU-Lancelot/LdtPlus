namespace LdtPlus.Config.RawData;
public record ConfigRaw
{
    public string? LdtPath { get; init; }
    public string? LdtVersion { get; init; }
    public string? LdtBookkitUrl { get; init; }

    public List<ConfigRawSection> Sections { get; init; } = [];
    public List<ConfigRawFavourite> Favourites { get; set; } = [];
    public ConfigRawHistory? History { get; set; }
}