using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuItemFavourite(
    string Name,
    string Command
) : IMenuRow
{
    public string Description => string.Empty;

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Menu.Command.Run, Command);
    }
}