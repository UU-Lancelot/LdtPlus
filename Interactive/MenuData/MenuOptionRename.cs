using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuOptionRename : IMenuItem
{
    public string Name => "Rename";

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        var separatorIndex = position.ActiveSelection.SelectedKey.LastIndexOf('~');
        var itemName = position.ActiveSelection.SelectedKey.Substring(0, separatorIndex);
        setCommand(Command.FavouriteRename, itemName);
    }
}
