namespace LdtPlus.Menu;
public record MenuRoot
(
    IEnumerable<MenuSection> Sections
) : IMenuContainer
{
    public IEnumerable<string> Navigation => ["Favourites", "Recent"];
}