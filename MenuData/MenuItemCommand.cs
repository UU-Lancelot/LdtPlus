namespace LdtPlus.MenuData;
public class MenuCommand : IMenuItem, IMenuContainer
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public IEnumerable<IMenuItem> Arguments { get; init; } = Enumerable.Empty<IMenuItem>();
    public IEnumerable<MenuSection> Sections => Enumerable.Repeat(new MenuSection("Arguments", Arguments), 1);
    public IEnumerable<string> ItemOptions => [];
    public IEnumerable<string> Navigation => ["Back", "Run"];
}