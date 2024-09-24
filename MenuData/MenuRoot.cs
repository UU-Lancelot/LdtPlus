namespace LdtPlus.MenuData;
public record MenuRoot : IMenuContainer
{
    public MenuRoot(IEnumerable<MenuSection> sections, IEnumerable<IMenuRow> favourites, IEnumerable<IMenuRow> recent)
    {
        Sections = sections;
        Navigation = [new MenuNavFavourites(favourites), new MenuNavRecent(recent)];
    }

    public IEnumerable<MenuSection> Sections { get; }
    public IEnumerable<IMenuItem> Navigation { get; }
    public IEnumerable<IMenuItem> ItemOptions => Enumerable.Empty<IMenuItem>();
}