using Spectre.Console.Rendering;

namespace LdtPlus.Gui.Interfaces;
public interface IComponent
{
    string Key { get; }
    IRenderable MainFrame { get; }
}
