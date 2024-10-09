
using LdtPlus.Gui.Tools;

namespace LdtPlus.MenuData;
public record MenuPath : IMenuContainer, IMenuRow
{
    public MenuPath(string fullPath)
    {
        FullPath = fullPath;
        Navigation = [string.IsNullOrEmpty(fullPath) ? new MenuNavExit() : new MenuNavUpper()];
    }

    public string FullPath { get; }
    public string Name => string.IsNullOrEmpty(Path.GetFileName(FullPath)) ? FullPath : Path.GetFileName(FullPath);
    public string Description => string.Empty;

    public IEnumerable<MenuSection> Sections
    {
        get
        {
            if (_sections == null)
                _sections = GetMenuSections(FullPath);

            return _sections;
        }
    }
    public IEnumerable<IMenuItem> Navigation { get; }
    public IEnumerable<IMenuItem> ItemOptions => [];

    private MenuSection[]? _sections;

    public void OnSelect(MenuPosition position, Action<Menu.Command, string> setCommand)
    {
        position.EnterSelected();
    }

    private MenuSection[] GetMenuSections(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
            return [new("Directories", DriveInfo.GetDrives().Select(d => new MenuPath(d.Name)).Prepend(new MenuPath("~")))];

        if (fullPath == "~")
            fullPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        return [
            new MenuSection("Directories", Directory.GetDirectories(fullPath).Select(d => new MenuPath(d))),
            new MenuSection("Files", Directory.GetFiles(fullPath).Select(f => new MenuPathFile(f))),
        ];
    }
}