namespace LdtPlus.Config.RawData;
public record ConfigRawMenu
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public ConfigRawMenuType? Type { get; init; }
    public List<ConfigRawSection> Sections { get; init; } = [];

    public ConfigRawMenuType GetMenuType()
    {
        if (Type is not null)
            return Type.Value;

        if (Sections.Count > 0)
            return ConfigRawMenuType.Area;

        return ConfigRawMenuType.Command;
    }
}
