using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuNavExit : IMenuItem
{
    public string Name => "Exit";
    public void OnSelect(Gui.Gui gui, MenuPosition pos, Action<Result> setResult)
    {
        setResult(new ResultQuit());
    }
}