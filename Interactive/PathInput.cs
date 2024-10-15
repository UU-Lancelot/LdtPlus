using LdtPlus.Interactive.MenuData;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive;
public class PathInput : Menu
{
    public PathInput(Gui.Gui gui, string path, bool fileOnly)
        : base(gui, CreateFromRoot(path, fileOnly))
    {
        _fileOnly = fileOnly;
    }

    private readonly bool _fileOnly;

    protected override void ShowMenu()
    {
        _gui.Show(batch => batch
            .ShowText($"Path: {string.Join("\\", _menuPosition.Path.Select(p => p.SimplifyName().Trim('\\')))}\\{_menuPosition.Filter}")
            .ShowMenu(_menuPosition));
    }

    public void RefreshMenu(string path)
    {
        _menuPosition = CreateFromRoot(path, _fileOnly);
    }

    private static MenuPosition CreateFromRoot(string fullPath, bool fileOnly)
    {
        MenuPath currentPath = new("", fileOnly);
        MenuPosition menuPosition = new(currentPath);

        string[] pathParts = SplitPath(fullPath);
        foreach (string pathPart in pathParts)
        {
            menuPosition.EnterKey(pathPart);
        }

        return menuPosition;
    }

    private static string[] SplitPath(string path)
    {
        string? currentPath = path;
        List<string> parts = new();

        while (currentPath != null)
        {
            var currentDirName = System.IO.Path.GetFileName(currentPath);
            parts.Add(string.IsNullOrEmpty(currentDirName) ? currentPath : currentDirName);
            currentPath = System.IO.Path.GetDirectoryName(currentPath);
        }

        return ((IEnumerable<string>)parts).Reverse().ToArray();
    }
}