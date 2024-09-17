namespace LdtPlus.MenuData;
public record MenuRoot
(
    IEnumerable<MenuSection> Sections
) : IMenuContainer
{
    public IEnumerable<IMenuNav> Navigation => [new MenuNavFavourites(), new MenuNavRecent()];
}