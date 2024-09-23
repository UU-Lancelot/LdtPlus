using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class InputComponent : IComponent, IDisposable
{
    internal InputComponent(IComponentContainer parent, string message, string input, string? key = null)
    {
        _parent = parent;
        Key = key ?? "input";
        MainFrame = new Text($"{message}: {input}");
    }

    private readonly IComponentContainer _parent;

    public string Key { get; }
    public IRenderable MainFrame { get;}

    public void Dispose()
    {
        _parent.Remove(this);
    }
}