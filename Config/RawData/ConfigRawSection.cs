namespace LdtPlus.Config.RawData;
public record ConfigRawSection
{
    public string? Title { get; init; }
    public List<ConfigRawMenu> Submenu { get; init; } = [];
}
