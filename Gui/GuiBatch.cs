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
        CommandComponent component = new(_guiBase, command);
        _components.Add(component);

        return this;
    }

    public GuiBatch ShowInput(string message, string input)
    {
        InputComponent component = new(_guiBase, message, input);
        _components.Add(component);

        return this;
    }

    public GuiBatch ShowPath()
    {
        return this;
    }

    public GuiBatch ShowMenu(MenuPosition menu)
    {
        MenuComponent component = new(_guiBase, menu);
        _components.Add(component);

        return this;
    }

    public GuiBatch ShowPathInput()
    {
        return this;
    }

    public GuiBatch ShowStream(Stream stream)
    {
        StreamComponent component = new(_guiBase, stream);
        _components.Add(component);

        return this;
    }
}