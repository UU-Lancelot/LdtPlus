using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuNavRecent(
    IEnumerable<IMenuRow> Recent
) : IMenuItem, IMenuContainer
{
    public string Name => "Recent";
    public IEnumerable<MenuSection> Sections => Enumerable.Repeat(new MenuSection("Recent", Recent), 1);
    public IEnumerable<IMenuItem> Navigation => [new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => [new MenuOptionAdd()];

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        position.EnterSelected();
    }
}