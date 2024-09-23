using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuItemRecent(
    string Name
) : IMenuRow
{
    public string Description => string.Empty;

    public IEnumerable<IMenuItem> ItemOptions => [new MenuOptionAdd()];

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.Run, Name);
    }
}