using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemFavourite(
    string Name,
    string Command
) : IMenuRow
{
    public string Description => string.Empty;

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Interactive.Command.Run, Command);
    }
}