using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemArea
(
    string Name,
    string Description,
    IEnumerable<MenuSection> Sections
) : IMenuRow, IMenuContainer
{
    public IEnumerable<IMenuItem> Navigation => [new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => Enumerable.Empty<IMenuItem>();

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        position.EnterSelected();
    }
}
