using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemArgumentText : IMenuRow
{
    public MenuItemArgumentText(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        Input input = new(gui);
        if (input.TryGetResult("Enter argument value", out string? value))
            position.Arguments.Add($"{Name.SimplifyName()} {value}");

        position.FilterClear();
    }
}