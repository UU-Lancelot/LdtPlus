using LdtPlus.MenuData;

namespace LdtPlus.Config;
public record ConfigData(
    IEnumerable<MenuSection> Sections,
    List<MenuItemFavourite> Favourites,
    List<MenuItemRecent> Recent
)
{
    public MenuRoot Menu => new(Sections, Favourites, Recent);
}