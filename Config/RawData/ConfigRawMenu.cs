namespace LdtPlus.Config.RawData;
public record ConfigRawMenu
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<ConfigRawSection> Sections { get; init; } = [];
}
