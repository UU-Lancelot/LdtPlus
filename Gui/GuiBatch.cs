using System.Diagnostics;
using LdtPlus.Gui.Interfaces;
using LdtPlus.Gui.Tools;

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

    public GuiBatch ShowCommand(string command)
    {
        CommandComponent component = new(command);
        _components.Add(component);

        return this;
    }

    public GuiBatch ShowInput(string message, string input)
    {
        InputComponent component = new(message, input);
        _components.Add(component);

        return this;
    }

    public GuiBatch ShowPath(string path)
    {
        PathComponent component = new(path);
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