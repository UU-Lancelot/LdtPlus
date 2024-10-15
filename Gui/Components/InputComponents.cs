using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class InputComponent : IComponent
{
    internal InputComponent(string message, string input, string? key = null)
    {
        Key = key ?? "input";
        MainFrame = new Text($"{message}: {input}");
    }

    public string Key { get; }
    public IRenderable MainFrame { get; }
}