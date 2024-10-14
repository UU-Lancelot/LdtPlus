using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuNavRun : IMenuItem
{
    public string Name => "Run";

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.Run, position.GenerateCommand());
    }
}