using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuNavBack : IMenuItem
{
    public string Name => "Back";
    public void OnSelect(Gui.Gui gui, MenuPosition pos, Action<Command, string> setResult)
    {
        pos.TryExit();
    }
}