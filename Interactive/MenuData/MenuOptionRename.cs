using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuOptionRename : IMenuItem
{
    public string Name => "Rename";

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        var separatorIndex = position.ActiveSelection.SelectedKey.LastIndexOf('~');
        var oldName = position.ActiveSelection.SelectedKey.Substring(0, separatorIndex);
        
        if (Input.TryGetResult(gui, "Favourite name", out string? newName))
            setResult(new ResultRenameFavourite(oldName, newName));
    }
}
