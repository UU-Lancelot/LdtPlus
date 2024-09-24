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
                IMenuRow? menuItem = currentMenu.Sections
                    .SelectMany(s => s.Submenu)
                    .Where(i => i.Name == key)
                    .FirstOrDefault();

                if (menuItem is not null && menuItem is IMenuContainer menuContainer)
                    currentMenu = menuContainer;
                else if (currentMenu.Navigation.FirstOrDefault(n => n.Name == key) is IMenuContainer navContainer)
                    currentMenu = navContainer;
                else
                    throw new InvalidOperationException($"Invalid path: {string.Join(" ",_currentPath)}");
            }

            return currentMenu;
        }
    }
    public IMenuItem? SelectedItem => CurrentMenu.Navigation.FirstOrDefault(n => n.Name == ActiveSelection.SelectedKey)
        ?? CurrentMenu.Sections.SelectMany(s => s.Submenu).FirstOrDefault(i => i.Name == ActiveSelection.SelectedKey)
        ?? CurrentMenu.Sections
            .SelectMany(s => s.Submenu) // all menu items
            .SelectMany(i => CurrentMenu.ItemOptions, (i, o) => (i, o)) // cartesian product
            .FirstOrDefault(pair => $"{pair.i.Name}_{pair.o.Name}" == ActiveSelection.SelectedKey)
            .o;
    public IEnumerable<MenuSection> SectionsFiltered => CurrentMenu.Sections
        .Select(s => new MenuSection(s.Title, s.Submenu.Where(m => m.Name.StartsWith(Filter, ignoreCase: true, null))))
        .Where(s => s.Submenu.Any());
    public IEnumerable<IMenuItem> NavigationFiltered => CurrentMenu.Navigation.Where(n => n.Name.StartsWith(Filter, ignoreCase: true, null));

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
            .SelectMany(s => s.Submenu.Select(i => CurrentMenu.ItemOptions.Select(o => $"{i.Name}_{o.Name}").Prepend(i.Name).ToArray()))
            .Prepend(NavigationFiltered.Select(n => n.Name).ToArray())
            .ToArray();
    }
}