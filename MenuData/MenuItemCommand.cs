using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuItemCommand(
    string Name,
    string Description
) : IMenuRow, IMenuContainer
{
    public IEnumerable<IMenuRow> Arguments { get; init; } = Enumerable.Empty<IMenuRow>();
    public IEnumerable<MenuSection> Sections => Enumerable.Repeat(new MenuSection("Arguments", Arguments), 1);
    public IEnumerable<IMenuItem> Navigation => [new MenuNavRun(), new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => Enumerable.Empty<IMenuItem>();

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        position.EnterSelected();
    }

}