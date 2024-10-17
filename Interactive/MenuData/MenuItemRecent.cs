using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemRecent(
    string Name
) : IMenuRow
{
    public string Description => string.Empty;

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        setResult(new ResultRun(Name));
    }
}