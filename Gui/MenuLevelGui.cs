using LdtPlus.Gui.Interfaces;
using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.Gui;
internal class MenuLevelGui : IMenuEventReceiver, IInputReceiver, IDisposable
{
    public MenuLevelGui(IComponentContainer parentComponent, IMenuContainer menu, MenuPosition position)
    {
        _position = position;
        _menuContainer = menu;
        _component = new(parentComponent, menu);
        _activeSelection = new(GetOptions(_menuContainer.Navigation, _menuContainer.Sections));

        _position.RegisterForEvents(this);
    }

    private readonly MenuPosition _position;
    private readonly IMenuContainer _menuContainer;
    private MenuLevelComponent _component;
    private ActiveSelection _activeSelection;

    private IEnumerable<MenuSection> SectionsFiltered => _menuContainer.Sections
        .Select(s => new MenuSection(s.Title, s.Submenu.Where(m => m.Name.StartsWith(_position.Filter, ignoreCase: true, null))))
        .Where(s => s.Submenu.Any());
    private IEnumerable<string> NavigationFiltered => _menuContainer.Navigation.Where(n => n.StartsWith(_position.Filter, ignoreCase: true, null));

    public void Enter(string key, IMenuContainer menu)
    {
        // do nothing
    }

    public void Exit(IMenuContainer menu)
    {
        // do nothing
    }

    public void UpdateFilter(string filter)
    {
        _activeSelection.UpdateOptions(GetOptions(NavigationFiltered, SectionsFiltered));
        _component.SetSelectedKey(_activeSelection.SelectedKey);
        _component.SetFilter(filter);
    }

    public void HandleInput(ConsoleKeyInfo keyInfo, out bool passToNextReceiver)
    {
        switch (keyInfo.Key)
        {
            // move
            case ConsoleKey.UpArrow:
                _activeSelection.MoveUp();
                _component.SetSelectedKey(_activeSelection.SelectedKey);
                passToNextReceiver = false;
                return;
            case ConsoleKey.DownArrow:
                _activeSelection.MoveDown();
                _component.SetSelectedKey(_activeSelection.SelectedKey);
                passToNextReceiver = false;
                return;
            case ConsoleKey.LeftArrow:
                _activeSelection.MoveLeft();
                _component.SetSelectedKey(_activeSelection.SelectedKey);
                passToNextReceiver = false;
                return;
            case ConsoleKey.RightArrow:
                _activeSelection.MoveRight();
                _component.SetSelectedKey(_activeSelection.SelectedKey);
                passToNextReceiver = false;
                return;

            // entry/exit
            case ConsoleKey.Enter:
            case ConsoleKey.Spacebar:
                // nothing selected
                if (_activeSelection.SelectedKey is null)
                    break;

                // selected navigation
                if (TryHandleNavigation(out passToNextReceiver))
                    return;

                _position.Enter(_activeSelection.SelectedKey);
                passToNextReceiver = false;
                return;
            case ConsoleKey.Escape:
                bool menuExited = _position.TryExit();
                passToNextReceiver = !menuExited;
                return;

            // filter
            case ConsoleKey.Backspace:
                _position.FilterRemoveLastChar();
                passToNextReceiver = false;
                return;
            default:
                // ignore invisible characters
                if (char.IsControl(keyInfo.KeyChar) || char.IsWhiteSpace(keyInfo.KeyChar))
                    break;

                _position.FilterAdd(keyInfo.KeyChar);
                passToNextReceiver = false;
                return;
        }

        passToNextReceiver = true;
    }

    private IEnumerable<IEnumerable<string>> GetOptions(IEnumerable<string> navigation, IEnumerable<MenuSection> sections)
    {
        IEnumerable<IEnumerable<string>> menu = sections
            .SelectMany(s => s.Submenu)
            .Select(m => (string[])[m.Name]);

        if (navigation.Any())
            return menu.Prepend(navigation);

        return menu;
    }

    private bool TryHandleNavigation(out bool passToNextReceiver)
    {
        if (!_menuContainer.Navigation.Contains(_activeSelection.SelectedKey))
        {
            passToNextReceiver = false;
            return false;
        }

        switch (_activeSelection.SelectedKey)
        {
            case "Back":
                bool menuExited = _position.TryExit();
                passToNextReceiver = !menuExited;
                return true;
            case "Run":
                passToNextReceiver = false;
                return true;
        }
    }

    public void Dispose()
    {
        _position.UnregisterForEvents(this);
        _component.Dispose();
    }
}