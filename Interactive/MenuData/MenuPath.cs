
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuPath : IMenuContainer, IMenuRow
{
    public MenuPath(string fullPath, bool fileOnly)
    {
        _fileOnly = fileOnly;
        FullPath = fullPath;
        Navigation = string.IsNullOrEmpty(fullPath)
            ? [new MenuNavExit()]
            : fileOnly
                ? [new MenuNavUpper()]
                : [new MenuNavUpper(), new MenuNavOk(fullPath)];
    }

    private readonly bool _fileOnly;
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

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        position.EnterSelected();
    }

    private MenuSection[] GetMenuSections(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
            return [new("Directories", DriveInfo.GetDrives().Select(d => new MenuPath(d.Name, _fileOnly)).Prepend(new MenuPath("~", _fileOnly)))];

        if (fullPath == "~")
            fullPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        return [
            new MenuSection("Directories", Directory.GetDirectories(fullPath).Select(d => new MenuPath(d, _fileOnly))),
            new MenuSection("Files", Directory.GetFiles(fullPath).Select(f => new MenuPathFile(f))),
        ];
    }
}