using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public class MenuNavBack : IMenuNav
{
    public string Name => "Back";
    public bool TryNavigate(MenuPosition pos, Action<Command> setResult)
    {
        pos.TryExit();
        return true;
    }
}