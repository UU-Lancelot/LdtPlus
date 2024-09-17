using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public class MenuNavFavourites : IMenuNav, IMenuContainer
{
    public string Name => "Favourites";
    public IEnumerable<IMenuRow> Arguments { get; init; } = Enumerable.Empty<IMenuRow>();
    public IEnumerable<MenuSection> Sections => Enumerable.Empty<MenuSection>();
    public IEnumerable<IMenuNav> Navigation => [new MenuNavBack()];

    public bool TryNavigate(MenuPosition position, Action<Command> setCommand)
    {
        return false;
    }
}