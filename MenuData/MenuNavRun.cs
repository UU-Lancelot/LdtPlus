using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuNavRun : IMenuItem
{
    public string Name => "Run";

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.Run, string.Join(" ", position.Path));
    }
}