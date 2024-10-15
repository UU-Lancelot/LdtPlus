using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemRecent(
    string Name
) : IMenuRow
{
    public string Description => string.Empty;

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.Run, Name);
    }
}