using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuOptionRename : IMenuItem
{
    public string Name => "Rename";

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.FavouriteRename, position.ActiveSelection.SelectedKey);
    }
}
