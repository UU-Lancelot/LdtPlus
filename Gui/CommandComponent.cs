using LdtPlus.Gui.Interfaces;
using LdtPlus.Gui.Tools;
using LdtPlus.Menu;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class CommandComponent : IComponent, IMenuEventReceiver
{
    public CommandComponent(IComponentContainer parent, MenuPosition menuPosition)
    {
        _parent = parent;
        _key = Guid.NewGuid().ToString();
        _menuPosition = menuPosition;
        _text = new Text(FullCommand);
    }

    private readonly IComponentContainer _parent;
    private readonly string _key;
    private MenuPosition _menuPosition;
    private Text _text;

    public IRenderable MainFrame => _text;
    public string FullCommand => string.Join(" ", _menuPosition.Path.Append(_menuPosition.Filter).Prepend("ldt"));

    public void Rerender()
    {
        _text = new(FullCommand);
        _parent.Update(_key, this);
    }

    public void Enter(string key, IMenuContainer menu)
    {
        Rerender();
    }

    public void Exit(IMenuContainer menu)
    {
        Rerender();
    }

    public void UpdateFilter(string filter)
    {
        Rerender();
    }
}
