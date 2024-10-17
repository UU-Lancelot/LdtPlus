using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class SubTitleComponent : IComponent
{
    internal SubTitleComponent(string bookkitUrl, string? key = null)
    {
        Key = key ?? "subtitle";
        MainFrame = new Rule($"uuBookKit: {bookkitUrl}");
    }

    public string Key { get; }
    public IRenderable MainFrame { get; }
}