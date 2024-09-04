using LdtPlus.Menu;
using Spectre.Console;

namespace LdtPlus.Gui;
public class MenuComponent : IInputReceiver
{
    public MenuComponent(IMenuContainer menuContainer, GuiMenu parent)
    {
        _menuContainer = menuContainer;
        _parent = parent;

        _filter = string.Empty;
        _navigation = GetNavigation(_menuContainer);
        _activeSelection = new(GetOptions(_navigation, _menuContainer.Sections));
        MainFrame = new()
        {
            ShowHeaders = false,
            Border = TableBorder.None,
        };
        MainFrame.AddColumn(string.Empty);
    }

    private readonly IMenuContainer _menuContainer;
    private readonly GuiMenu _parent;
    private string _filter;
    private readonly string[] _navigation;
    private readonly ActiveSelection _activeSelection;
    private IEnumerable<MenuSection> SectionsFiltered => _menuContainer.Sections
        .Select(s => new MenuSection(s.Title, s.Submenu.Where(m => m.Name.StartsWith(_filter, ignoreCase: true, null))))
        .Where(s => s.Submenu.Any());
    private IEnumerable<string> NavigationFiltered => _navigation.Where(n => n.StartsWith(_filter, ignoreCase: true, null));

    public Table MainFrame { get; }
    public string ActiveKey => _activeSelection.SelectedKey;

    public void UpdateMainFrame()
    {
        // clear previous
        MainFrame.Rows.Clear();

        // add navigation
        if (NavigationFiltered.Any())
        {
            Grid navigation = new();
            foreach (string _ in NavigationFiltered)
            {
                navigation.AddColumn().AddColumn(); // select, nav
            }
            navigation.AddRow(NavigationFiltered.SelectMany<string, string>(n => [n == ActiveKey ? ">" : " ", n]).ToArray());
            MainFrame.AddRow(navigation);
        }

        // add all sections
        foreach (MenuSection section in SectionsFiltered)
        {
            Text sectionTitle = new(section.Title);
            MainFrame.AddRow(sectionTitle);

            Grid sectionGrid = new();
            sectionGrid.AddColumn().AddColumn().AddColumn(); // select, name, description
            foreach (IMenuItem item in section.Submenu)
            {
                sectionGrid.AddRow([item.Name == ActiveKey ? ">" : " ", item.Name, item.Description]);
            }
            MainFrame.AddRow(sectionGrid);
        }

        _parent.Refresh();
    }

    private string[] GetNavigation(IMenuContainer menuContainer)
    {
        if (menuContainer is MenuRoot)
            return ["Favourites", "Recent"];
        if (menuContainer is MenuSection)
            return ["Back"];
        if (menuContainer is MenuCommand)
            return ["Run", "Back"];
        else
            throw new NotImplementedException();
    }

    private IEnumerable<IEnumerable<string>> GetOptions(IEnumerable<string> navigation, IEnumerable<MenuSection> sections)
    {
        IEnumerable<IEnumerable<string>> menu = sections
            .SelectMany(s => s.Submenu)
            .Select(m => (string[])[ m.Name ]);

        if (navigation.Any())
            return menu.Prepend(navigation);

        return menu;
    }

    private void UpdateFilter(string newValue)
    {
        _filter = newValue;
        _activeSelection.UpdateOptions(GetOptions(NavigationFiltered, SectionsFiltered));
        _parent.UpdateShowedCommand(_filter);
    }

    public void HandleInput(ConsoleKeyInfo keyInfo, out bool passToNextReceiver)
    {
        passToNextReceiver = false; // default

        switch (keyInfo.Key)
        {
            // menu navigation
            case ConsoleKey.UpArrow:
                _activeSelection.MoveUp();
                UpdateMainFrame();
                return;
            case ConsoleKey.DownArrow:
                _activeSelection.MoveDown();
                UpdateMainFrame();
                return;
            case ConsoleKey.LeftArrow:
                _activeSelection.MoveLeft();
                UpdateMainFrame();
                return;
            case ConsoleKey.RightArrow:
                _activeSelection.MoveRight();
                UpdateMainFrame();
                return;
            case ConsoleKey.Enter:
            case ConsoleKey.Spacebar:
                string selectedKey = _activeSelection.SelectedKey;
                IMenuItem selectedItem = _menuContainer.Sections
                    .SelectMany(s => s.Submenu)
                    .FirstOrDefault(m => m.Name == selectedKey)
                    ?? throw new InvalidOperationException("Selected item not found");
                if (selectedItem is IMenuContainer submenu)
                    _parent.EnterMenu(submenu);
#warning TODO
                // else if (selectedItem is MenuCommand menuCommand)
                //     _parent.RunCommand(menuCommand);
                else
                    throw new NotImplementedException("Unknown menu item type");
                return;
            case ConsoleKey.Escape:
                _parent.ExitMenu(out bool menuExited);
                passToNextReceiver = !menuExited;
                return;

            // filter
            case ConsoleKey.Backspace:
                string newFilter = _filter.Length > 0 ? _filter[..^1] : string.Empty;
                UpdateFilter(newFilter);
                UpdateMainFrame();
                return;
            default:
                // ignore invisible characters
                if (char.IsControl(keyInfo.KeyChar) || char.IsWhiteSpace(keyInfo.KeyChar))
                    return;

                UpdateFilter($"{_filter}{keyInfo.KeyChar}");
                UpdateMainFrame();
                return;
        }
    }
}
