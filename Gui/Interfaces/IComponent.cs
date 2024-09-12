using Spectre.Console.Rendering;

namespace LdtPlus.Gui.Interfaces;
public interface IComponent
{
    IRenderable MainFrame { get; }

    void Rerender();
}
