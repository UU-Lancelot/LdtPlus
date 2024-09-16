using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class TitleComponent : IComponent, IDisposable
{
    internal TitleComponent(IComponentContainer parent, string? key = null)
    {
        _parent = parent;
        Key = key ?? "title";
        MainFrame = new Rule("LancelotDeployTool+");
    }

    private readonly IComponentContainer _parent;

    public string Key { get; }
    public IRenderable MainFrame { get;}

    public void Dispose()
    {
        _parent.Remove(this);
    }
}