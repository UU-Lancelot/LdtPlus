using LdtPlus.Gui.Interfaces;
using LdtPlus.Menu;

namespace LdtPlus.Gui.Tools;
internal class MenuPosition
{
    public MenuPosition(MenuRoot menuRoot)
    {
        _menuRoot = menuRoot;
        _currentPath = new();
        _currentFilter = string.Empty;
        _receivers = new();
    }

    private readonly MenuRoot _menuRoot;
    private Stack<string> _currentPath;
    private string _currentFilter;
    private List<IMenuEventReceiver> _receivers;

    public IEnumerable<string> Path => _currentPath;
    public string Filter => _currentFilter;
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

    public void RegisterForEvents(IMenuEventReceiver receiver)
    {
        _receivers.Add(receiver);
    }

    public void UnregisterForEvents(IMenuEventReceiver receiver)
    {
        _receivers.Remove(receiver);
    }

    public void Enter(string key)
    {
        _currentPath.Push(key);
        _currentFilter = string.Empty;

        IMenuContainer menu = CurrentMenu;
        foreach (var receiver in _receivers)
        {
            receiver.Enter(key, menu);
        }
    }

    public bool TryExit()
    {
        if (!_currentPath.TryPop(out _))
            return false;

        _currentFilter = string.Empty;

        IMenuContainer menu = CurrentMenu;
        foreach (var receiver in _receivers)
        {
            receiver.Exit(menu);
        }
        return true;
    }

    public void FilterAdd(char c)
    {
        _currentFilter += c;

        foreach (var receiver in _receivers)
        {
            receiver.UpdateFilter(_currentFilter);
        }
    }
    public void FilterRemoveLastChar()
    {
        if (_currentFilter.Length == 0)
            return;

        _currentFilter = _currentFilter[..^1];

        foreach (var receiver in _receivers)
        {
            receiver.UpdateFilter(_currentFilter);
        }
    }
}