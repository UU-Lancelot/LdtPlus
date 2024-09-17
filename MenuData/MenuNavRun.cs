using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public class MenuNavRun : IMenuNav
{
    public string Name => "Run";

    bool IMenuNav.TryNavigate(MenuPosition position, Action<Command> setCommand)
    {
        setCommand(Command.Run);
        return true;
    }
}