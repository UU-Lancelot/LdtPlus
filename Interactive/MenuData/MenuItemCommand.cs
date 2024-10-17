using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemCommand(
    string Name,
    string Description,
    IEnumerable<MenuSection> ArgumentSections
) : IMenuRow, IMenuContainer
{
    public IEnumerable<MenuSection> Sections => ArgumentSections;
    public IEnumerable<IMenuItem> Navigation => [new MenuNavRun(), new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => Enumerable.Empty<IMenuItem>();

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        position.EnterSelected();
    }

}