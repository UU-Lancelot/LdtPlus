using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuOptionDelete : IMenuItem
{
    public string Name => "Delete";

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.FavouriteDelete, position.ActiveSelection.SelectedKey);
    }
}
