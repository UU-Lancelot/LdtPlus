using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.MenuData;
public record MenuOptionAdd : IMenuItem
{
    public string Name => "Add to favourites";

    public void OnSelect(MenuPosition position, Action<Command, string> setCommand)
    {
        setCommand(Command.FavouriteAdd, position.ActiveSelection.SelectedKey);
    }
}
