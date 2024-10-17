using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuOptionDelete : IMenuItem
{
    public string Name => "Delete";

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        var separatorIndex = position.ActiveSelection.SelectedKey.LastIndexOf('~');
        var itemName = position.ActiveSelection.SelectedKey.Substring(0, separatorIndex);

        setResult(new ResultDeleteFavourite(itemName));
    }
}
