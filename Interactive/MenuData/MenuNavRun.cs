using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuNavRun : IMenuItem
{
    public string Name => "Run";

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.Run, position.GenerateCommand());
    }
}