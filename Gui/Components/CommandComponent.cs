using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class CommandComponent : IComponent, IDisposable
{
    internal CommandComponent(IComponentContainer parent, string commandText)
    {
        _parent = parent;
        Key = "command";
        MainFrame = new Text(commandText);
    }

    private readonly IComponentContainer _parent;

    public string Key { get; }
    public IRenderable MainFrame { get;}

    public void Dispose()
    {
        _parent.Remove(this);
    }
}