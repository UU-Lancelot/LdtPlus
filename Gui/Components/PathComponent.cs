using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class PathComponent : IComponent, IDisposable
{
    internal PathComponent(IComponentContainer parent, string commandText, string? key = null)
    {
        _parent = parent;
        Key = key ?? "path input";
        MainFrame = new Text($"Path: {commandText.Replace("\\\\", "\\")}");
    }

    private readonly IComponentContainer _parent;

    public string Key { get; }
    public IRenderable MainFrame { get;}

    public void Dispose()
    {
        _parent.Remove(this);
    }
}