using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
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