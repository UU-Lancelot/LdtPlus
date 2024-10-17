using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuOptionAdd : IMenuItem
{
    public string Name => "Add to favourites";

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        int separatorIndex = position.ActiveSelection.SelectedKey.LastIndexOf('~');
        string command = position.ActiveSelection.SelectedKey.Substring(0, separatorIndex);

        if (Input.TryGetResult(gui, "Favourite name", out string? name))
            setResult(new ResultAddFavourite(name, command));
    }
}
