using LdtPlus.Menu;

namespace LdtPlus.Gui.Interfaces;
public interface IMenuEventReceiver
{
    void Enter(string key, IMenuContainer menu);
    void Exit(IMenuContainer menu);
    void UpdateFilter(string filter);
}
