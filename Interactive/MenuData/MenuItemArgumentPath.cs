using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemArgumentPath : IMenuRow
{
    public MenuItemArgumentPath(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        string currentDir = Environment.CurrentDirectory;
        PathInput pathMenu = new(gui, currentDir, fileOnly: false);
        Result result = pathMenu.GetCommand();
        if (result is ResultSelectPath pathResult)
            position.Arguments.Add($"{Name.SimplifyName()} {pathResult.Path}");

        position.FilterClear();
    }
}