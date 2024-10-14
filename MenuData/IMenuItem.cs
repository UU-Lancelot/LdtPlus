using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public interface IMenuItem
{
    string Name { get; }
    void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand);
}
