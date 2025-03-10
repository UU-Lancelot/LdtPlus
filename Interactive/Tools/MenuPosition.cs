using LdtPlus.Interactive.MenuData;

namespace LdtPlus.Interactive.Tools;
public class MenuPosition
{
    public MenuPosition(IMenuContainer menuRoot)
    {
        _menuRoot = menuRoot;
        _currentPath = new();
        _activeSelections = new();
        Arguments = new();
        Filter = string.Empty;

        _activeSelections.Push(new ActiveSelection(GetOptions()));
    }

    private readonly IMenuContainer _menuRoot;
    private Stack<string> _currentPath;
    private readonly Stack<ActiveSelection> _activeSelections;

    public ActiveSelection ActiveSelection => _activeSelections.Peek();

    public IEnumerable<string> Path => _currentPath.Reverse();
    public List<string> Arguments { get; }
    public string Filter { get; private set; }
    public IMenuContainer CurrentMenu
    {
        get
        {
            IMenuContainer currentMenu = _menuRoot;
            foreach (var key in Path)
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
                    throw new InvalidOperationException($"Invalid path: {string.Join(" ", Path)}");
            }

            return currentMenu;
        }
    }
    public IMenuItem? SelectedItem => CurrentMenu.Navigation.FirstOrDefault(n => n.Name == ActiveSelection.SelectedKey)
        ?? CurrentMenu.Sections.SelectMany(s => s.Submenu).FirstOrDefault(i => i.Name == ActiveSelection.SelectedKey)
        ?? CurrentMenu.Sections
            .SelectMany(s => s.Submenu) // all menu items
            .SelectMany(i => CurrentMenu.ItemOptions, (i, o) => (i, o)) // cartesian product
            .FirstOrDefault(pair => $"{pair.i.Name}~{pair.o.Name}" == ActiveSelection.SelectedKey)
            .o;
    public IEnumerable<MenuSection> SectionsFiltered => CurrentMenu.Sections
        .Select(s => new MenuSection(s.Title, s.Submenu.Where(m => m.Name.SplitName().Any(n => n.StartsWith(Filter, ignoreCase: true, null)))))
        .Where(s => s.Submenu.Any());
    public IEnumerable<IMenuItem> NavigationFiltered => CurrentMenu.Navigation.Where(n => n.Name.SplitName().Any(n => n.StartsWith(Filter, ignoreCase: true, null)));

    public void EnterSelected()
    {
        _currentPath.Push(ActiveSelection.SelectedKey);
        Filter = string.Empty;

        _activeSelections.Push(new ActiveSelection(GetOptions()));
    }
    public void EnterKey(string key)
    {
        // get current options
        ActiveSelection current = _activeSelections.Peek();
        string[][] options = GetOptions();

        // filter only single option
        current.UpdateOptions([[key]]);
        EnterSelected();

        // restore options
        current.UpdateOptions(options);
    }
    public bool TryExit()
    {
        if (!_currentPath.TryPop(out _))
            return false;

        Filter = string.Empty;
        _activeSelections.Pop();

        if (CurrentMenu is MenuItemArea || CurrentMenu is MenuRoot)
            Arguments.Clear();

        return true;
    }

    public void FilterAdd(char c)
    {
        Filter += c;

        ActiveSelection current = _activeSelections.Peek();
        current.UpdateOptions(GetOptions());
    }
    public bool TryFilterRemoveLastChar()
    {
        if (!Filter.Any())
            return false;

        Filter = Filter[..^1];

        ActiveSelection current = _activeSelections.Peek();
        current.UpdateOptions(GetOptions());
        return true;
    }
    public void FilterClear()
    {
        Filter = string.Empty;

        ActiveSelection current = _activeSelections.Peek();
        current.UpdateOptions(GetOptions());
    }

    public string GenerateCommand()
    {
        string path = string.Join(" ", Path.Select(p => p.SimplifyName()));
        string arguments = string.Join(" ", Arguments);
        string[] parts = [path, arguments, Filter];

        return string.Join(" ", parts.Where(s => !string.IsNullOrEmpty(s)));
    }

    private string[][] GetOptions()
    {
        IMenuContainer currentMenu = CurrentMenu;
        return SectionsFiltered
            .SelectMany(s => s.Submenu.Select(i => currentMenu.ItemOptions.Select(o => $"{i.Name}~{o.Name}").Prepend(i.Name).ToArray()))
            .Prepend(NavigationFiltered.Select(n => n.Name).ToArray())
            .Where(s => s.Any())
            .ToArray();
    }
}