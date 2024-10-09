using LdtPlus.Gui.Tools;
using LdtPlus.MenuData;

namespace LdtPlus.Menu;
public class PathInput : Menu
{
    public PathInput(Gui.Gui gui, string path)
        : base(gui, CreateFromRoot(path))
    {
    }

    protected override void ShowMenu()
    {
        _gui.Show(batch => batch
            .ShowPath($"{string.Join("\\", _menuPosition.Path.Select(p => p.Split(',')[0]))}\\{_menuPosition.Filter}")
            .ShowMenu(_menuPosition));
    }

    public void RefreshMenu(string path)
    {
        _menuPosition = CreateFromRoot(path);
    }

    private static MenuPosition CreateFromRoot(string fullPath)
    {
        MenuPath currentPath = new("");
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