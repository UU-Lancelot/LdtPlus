using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public class MenuNavRecent : IMenuNav, IMenuContainer
{
    public string Name => "Recent";
    public IEnumerable<IMenuRow> Recent { get; init; } = Enumerable.Empty<IMenuRow>();
    public IEnumerable<MenuSection> Sections => Enumerable.Repeat(new MenuSection("Recent", Recent), 1);
    public IEnumerable<IMenuNav> Navigation => [new MenuNavBack()];

    public bool TryNavigate(MenuPosition position, Action<Command> setCommand)
    {
        return false;
    }
}