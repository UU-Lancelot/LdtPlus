namespace LdtPlus.Menu;
public record MenuItemWithSubmenu
(
    string Name,
    string Description,
    IEnumerable<MenuSection> Sections
) : IMenuItem, IMenuContainer;
