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

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        string currentDir = Environment.CurrentDirectory;
        PathInput pathMenu = new(gui, currentDir, fileOnly: false);
        Command pathCommand = pathMenu.GetCommand(out string? pathParameter);
        if (pathCommand == Command.Run)
            position.Arguments.Add($"{Name.SimplifyName()} {pathParameter}");

        position.FilterClear();
    }
}