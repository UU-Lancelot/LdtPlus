using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class LoaderComponent : IComponent, IDisposable
{
    internal LoaderComponent(IComponentContainer parent)
    {
        _parent = parent;
        Key = Guid.NewGuid().ToString();
        MainFrame = new Text("Loading...");
    }

    private readonly IComponentContainer _parent;

    public string Key { get; }
    public IRenderable MainFrame { get;}

    public void Dispose()
    {
        _parent.Remove(this);
    }
}