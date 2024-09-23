using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuNavBack : IMenuItem
{
    public string Name => "Back";
    public void OnSelect(MenuPosition pos, Action<Command, string> setResult)
    {
        pos.TryExit();
    }
}