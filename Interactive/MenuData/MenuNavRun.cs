using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuNavRun : IMenuItem
{
    public string Name => "Run";

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        setResult(new ResultRun(position.GenerateCommand()));
    }
}