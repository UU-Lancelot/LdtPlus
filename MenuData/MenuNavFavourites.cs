using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuNavFavourites(
    IEnumerable<IMenuRow> Favourites
) : IMenuItem, IMenuContainer
{
    public string Name => "Favourites";
    public IEnumerable<MenuSection> Sections => Enumerable.Repeat(new MenuSection("Favourites", Favourites), 1);
    public IEnumerable<IMenuItem> Navigation => [new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => [new MenuOptionRename(), new MenuOptionDelete()];

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        position.EnterSelected();
    }
}