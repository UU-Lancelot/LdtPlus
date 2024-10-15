using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class CommandComponent : IComponent
{
    internal CommandComponent(string commandText, string? key = null)
    {
        Key = key ?? "command";
        MainFrame = new Text($"Command: ldt {commandText}");
    }

    public string Key { get; }
    public IRenderable MainFrame { get; }
}