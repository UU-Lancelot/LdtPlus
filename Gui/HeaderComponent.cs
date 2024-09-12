using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class HeaderComponent : IComponent
{
    public HeaderComponent(IComponentContainer parent)
    {
        _parent = parent;
        _key = "header";
        MainFrame = new Rule("LancelotDeploymentTool");
    }

    private readonly IComponentContainer _parent;
    private string _key;

    public IRenderable MainFrame { get; }

    public void Rerender()
    {
        _parent.Update(_key, this);
    }
}