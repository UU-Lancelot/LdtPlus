using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuNavOk(
    string Path
) : IMenuItem
{
    public string Name => "Ok";
    public void OnSelect(Gui.Gui gui, MenuPosition pos, Action<Command, string> setResult)
    {
        setResult(Command.Run, Path);
    }
}