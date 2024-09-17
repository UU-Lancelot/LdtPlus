namespace LdtPlus.MenuData;
public class MenuCommand : IMenuRow, IMenuContainer
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public IEnumerable<IMenuRow> Arguments { get; init; } = Enumerable.Empty<IMenuRow>();
    public IEnumerable<MenuSection> Sections => Enumerable.Repeat(new MenuSection("Arguments", Arguments), 1);
    public IEnumerable<IMenuNav> Navigation => [new MenuNavRun(), new MenuNavBack()];
    public IEnumerable<string> ItemOptions => [];
}