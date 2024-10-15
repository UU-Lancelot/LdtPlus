using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class LineComponent : IComponent
{
    internal LineComponent(string text, string? key = null)
    {
        Key = key ?? "line";
        MainFrame = new Text(text);
    }

    public string Key { get; }
    public IRenderable MainFrame { get; }
}