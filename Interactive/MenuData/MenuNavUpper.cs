using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuNavUpper : IMenuItem
{
    public string Name => "..";
    public void OnSelect(Gui.Gui gui, MenuPosition pos, Action<Result> setResult)
    {
        pos.TryExit();
    }
}