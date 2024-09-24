using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuItemArea
(
    string Name,
    string Description,
    IEnumerable<MenuSection> Sections
) : IMenuRow, IMenuContainer
{
    public IEnumerable<IMenuItem> Navigation => [new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => Enumerable.Empty<IMenuItem>();

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        position.EnterSelected();
    }
}
