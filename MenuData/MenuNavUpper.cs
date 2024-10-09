using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuNavUpper : IMenuItem
{
    public string Name => "..";
    public void OnSelect(MenuPosition pos, Action<Command, string> setResult)
    {
        pos.TryExit();
    }
}