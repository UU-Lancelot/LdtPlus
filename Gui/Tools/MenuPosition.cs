using LdtPlus.MenuData;

namespace LdtPlus.Gui.Tools;
public class MenuPosition
{
    public MenuPosition(MenuRoot menuRoot)
    {
        _menuRoot = menuRoot;
        _currentPath = new();
        _activeSelections = new();
        Filter = string.Empty;

        _activeSelections.Push(new ActiveSelection(GetOptions()));
    }

    private readonly MenuRoot _menuRoot;
    private Stack<string> _currentPath;
    private readonly Stack<ActiveSelection> _activeSelections;

    public ActiveSelection ActiveSelection => _activeSelections.Peek();

    public IEnumerable<string> Path => _currentPath;
    public string Filter { get; private set; }
    public IMenuContainer CurrentMenu
    {
        get
        {
            IMenuContainer currentMenu = _menuRoot;
            foreach (var key in _currentPath)
            {
                IMenuItem? menuItem = currentMenu.Sections
                    .SelectMany(s => s.Submenu)
                    .Where(i => i.Name == key)
                    .FirstOrDefault();

                if (menuItem is not null && menuItem is IMenuContainer menuContainer)
                    currentMenu = menuContainer;
                else
                {
#warning TODO: warn about invalid path
                    break;
                }
            }

            return currentMenu;
        }
    }
    public IEnumerable<MenuSection> SectionsFiltered => CurrentMenu.Sections
        .Select(s => new MenuSection(s.Title, s.Submenu.Where(m => m.Name.StartsWith(Filter, ignoreCase: true, null))))
        .Where(s => s.Submenu.Any());
    public IEnumerable<string> NavigationFiltered => CurrentMenu.Navigation.Where(n => n.StartsWith(Filter, ignoreCase: true, null));

    public void EnterSelected()
    {
        _currentPath.Push(ActiveSelection.SelectedKey);
        _activeSelections.Push(new ActiveSelection(GetOptions()));
        Filter = string.Empty;
    }
    public bool TryExit()
    {
        if (!_currentPath.TryPop(out _))
            return false;

        Filter = string.Empty;
        _activeSelections.Pop();

        return true;
    }

    public void FilterAdd(char c)
    {
        Filter += c;
    }
    public bool TryFilterRemoveLastChar()
    {
        if (!Filter.Any())
            return false;

        Filter = Filter[..^1];
        return true;
    }


    private string[][] GetOptions()
    {
        return SectionsFiltered
            .SelectMany(s => s.Submenu.Select<IMenuItem, string[]>(i => [i.Name]))
            .Prepend(NavigationFiltered.ToArray())
            .ToArray();
    }
}