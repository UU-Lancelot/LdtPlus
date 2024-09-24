using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuOptionDelete : IMenuItem
{
    public string Name => "Delete";

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        var separatorIndex = position.ActiveSelection.SelectedKey.LastIndexOf('~');
        var itemName = position.ActiveSelection.SelectedKey.Substring(0, separatorIndex);
        setCommand(Command.FavouriteDelete, itemName);
    }
}
