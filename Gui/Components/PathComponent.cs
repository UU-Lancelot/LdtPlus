using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class PathComponent : IComponent
{
    internal PathComponent(string commandText, string? key = null)
    {
        Key = key ?? "path input";
        MainFrame = new Text($"Path: {commandText.Replace("\\\\", "\\")}");
    }

    public string Key { get; }
    public IRenderable MainFrame { get; }
}