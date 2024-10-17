
using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuPathFile(
    string FullPath
) : IMenuRow
{
    public string Name => Path.GetFileName(FullPath) ?? FullPath;
    public string Description => string.Empty;

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Result> setResult)
    {
        setResult(new ResultSelectPath(FullPath));
    }
}