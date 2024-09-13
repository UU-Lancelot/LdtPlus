namespace LdtPlus.MenuData;
public record MenuRoot
(
    IEnumerable<MenuSection> Sections
) : IMenuContainer
{
    public IEnumerable<string> ItemOptions => [];
    public IEnumerable<string> Navigation => ["Favourites", "Recent"];
}