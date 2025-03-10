using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuNavRecent(
    IEnumerable<IMenuRow> Recent
) : IMenuItem, IMenuContainer
{
    public string Name => "Recent";
    public IEnumerable<MenuSection> Sections => Enumerable.Repeat(new MenuSection("Recent", Recent), 1);
    public IEnumerable<IMenuItem> Navigation => [new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => [new MenuOptionAdd()];

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        position.EnterSelected();
    }
}