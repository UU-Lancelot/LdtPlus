namespace LdtPlus.MenuData;
public record MenuItemArea
(
    string Name,
    string Description,
    IEnumerable<MenuSection> Sections
) : IMenuRow, IMenuContainer
{
    public IEnumerable<string> ItemOptions => [];
    public IEnumerable<IMenuNav> Navigation => [new MenuNavBack()];
}
