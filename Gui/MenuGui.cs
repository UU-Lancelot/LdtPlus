using LdtPlus.Gui.Interfaces;
using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.Gui;
internal class MenuGui : IMenuEventReceiver
{
    public MenuGui(IComponentContainer parent, MenuPosition menuPosition)
    {
        _parent = parent;
        _menuPosition = menuPosition;

        _component = new(_parent);
        _levels = new();

        _menuPosition.RegisterForEvents(this);
    }

    private readonly IComponentContainer _parent;
    private readonly MenuPosition _menuPosition;
    private MenuComponent _component;
    private Stack<MenuLevelGui> _levels;

    public void Show()
    {
        _component.Rerender();
    }

    public void Enter(string key, IMenuContainer menu)
    {
        MenuLevelGui level = new(_component, menu, _menuPosition);
        _levels.Push(level);
    }

    public void Exit(IMenuContainer menu)
    {
        MenuLevelGui level = _levels.Pop();
        level.Dispose();
    }

    public void UpdateFilter(string filter)
    {
        // do nothing
    }
}
