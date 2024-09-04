namespace LdtPlus.Menu;
public record MenuRoot
(
    IEnumerable<MenuSection> Sections
) : IMenuContainer;