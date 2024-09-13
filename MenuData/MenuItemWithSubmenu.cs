namespace LdtPlus.MenuData;
public record MenuItemWithSubmenu
(
    string Name,
    string Description,
    IEnumerable<MenuSection> Sections
) : IMenuItem, IMenuContainer
{
    public IEnumerable<string> ItemOptions => [];
    public IEnumerable<string> Navigation => ["Back"];
}
