using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuItemArgumentText : IMenuRow
{
    public MenuItemArgumentText(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        Input input = new(gui);
        if (input.TryGetResult("Enter argument value", out string? value))
            position.Arguments.Add($"{Name.SimplifyName()} {value}");

        position.FilterClear();
    }
}