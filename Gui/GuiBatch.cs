using System.Diagnostics;
using LdtPlus.Gui.Interfaces;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Gui;
public class GuiBatch
{
    internal GuiBatch(GuiBase guiBase, List<IComponent> components)
    {
        _guiBase = guiBase;
        _components = components;
    }

    private readonly GuiBase _guiBase;
    private List<IComponent> _components;

    public GuiBatch ShowText(string text)
    {
        LineComponent component = new(text);
        _components.Add(component);

        return this;
    }

    public GuiBatch ShowMenu(MenuPosition menu)
    {
        MenuComponent component = new(menu);
        _components.Add(component);

        return this;
    }

    public GuiBatch ShowProcess(Process process)
    {
        ProcessComponent component = new(_guiBase, process);
        _components.Add(component);

        return this;
    }
}