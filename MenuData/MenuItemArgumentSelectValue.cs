using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuItemArgumentSelectValue : IMenuRow
{
    public MenuItemArgumentSelectValue(string name, MenuItemArgumentSelect parent)
    {
        _parent = parent;
        Name = name;
    }

    private readonly MenuItemArgumentSelect _parent;

    public string Name { get; }
    public string Description => string.Empty;

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        position.Arguments.Add($"{_parent.Name.SimplifyName()} {Name}");
        position.TryExit();
        position.ActiveSelection.ResetPosition();
    }
}
