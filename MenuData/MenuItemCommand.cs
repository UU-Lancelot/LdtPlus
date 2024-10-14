using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuItemCommand(
    string Name,
    string Description,
    IEnumerable<MenuSection> ArgumentSections
) : IMenuRow, IMenuContainer
{
    public IEnumerable<MenuSection> Sections => ArgumentSections;
    public IEnumerable<IMenuItem> Navigation => [new MenuNavRun(), new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => Enumerable.Empty<IMenuItem>();

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        position.EnterSelected();
    }

}