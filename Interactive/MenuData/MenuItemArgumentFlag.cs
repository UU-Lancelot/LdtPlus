using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemArgumentFlag : IMenuRow
{
    public MenuItemArgumentFlag(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        position.Arguments.Add(Name);
    }
}