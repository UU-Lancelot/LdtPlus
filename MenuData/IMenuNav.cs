using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public interface IMenuNav : IMenuItem
{
    bool TryNavigate(MenuPosition position, Action<Command> setCommand);
}