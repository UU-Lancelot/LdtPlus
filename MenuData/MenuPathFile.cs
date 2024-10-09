
using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuPathFile(
    string FullPath
) : IMenuRow
{
    public string Name => Path.GetFileName(FullPath) ?? FullPath;
    public string Description => string.Empty;

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.Run, FullPath);
    }
}