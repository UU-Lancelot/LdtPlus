using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public interface IMenuItem
{
    string Name { get; }
    void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand);
}
