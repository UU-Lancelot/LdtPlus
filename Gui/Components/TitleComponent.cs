using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class TitleComponent : IComponent
{
    internal TitleComponent(string? key = null)
    {
        Key = key ?? "title";
        MainFrame = new Rule("LancelotDeployTool+");
    }

    public string Key { get; }
    public IRenderable MainFrame { get; }
}