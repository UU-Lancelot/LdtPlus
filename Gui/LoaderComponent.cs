using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class LoaderComponent : IComponent, IDisposable
{
    internal LoaderComponent(IComponentContainer parent)
    {
        _parent = parent;
        _key = Guid.NewGuid().ToString();
        _mainFrame = new Text("Loading...");
    }

    private readonly IComponentContainer _parent;
    private readonly string _key;
    private Text _mainFrame;

    public IRenderable MainFrame => _mainFrame;

    public void Rerender()
    {
        _parent.Update(_key, this);
    }

    public void Dispose()
    {
        _parent.Remove(_key);
    }

    internal static LoaderComponent ShowNew(IComponentContainer parent)
    {
        LoaderComponent loader = new(parent);
        loader.Rerender();

        return loader;
    }
}