using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class ErrorComponent : IComponent
{
    public ErrorComponent(IComponentContainer parent, string message)
    {
        _parent = parent;
        _key = "error";
        _rows = new Rows(new Rule("Error"), new Text(message));
    }

    private readonly IComponentContainer _parent;
    private string _key;
    private Rows _rows;

    public IRenderable MainFrame => _rows;

    public void Rerender()
    {
        _parent.Update(_key, this);
    }
}