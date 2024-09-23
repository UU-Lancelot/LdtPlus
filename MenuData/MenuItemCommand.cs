using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuCommand : IMenuRow, IMenuContainer
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public IEnumerable<IMenuRow> Arguments { get; init; } = Enumerable.Empty<IMenuRow>();
    public IEnumerable<MenuSection> Sections => Enumerable.Repeat(new MenuSection("Arguments", Arguments), 1);
    public IEnumerable<IMenuItem> Navigation => [new MenuNavRun(), new MenuNavBack()];
    public IEnumerable<IMenuItem> ItemOptions => [];

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        position.EnterSelected();
    }

}