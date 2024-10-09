using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuNavExit : IMenuItem
{
    public string Name => "Exit";
    public void OnSelect(MenuPosition pos, Action<Command, string> setResult)
    {
        setResult(Command.Exit, "");
    }
}