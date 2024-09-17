using LdtPlus.Gui.Tools;
using LdtPlus.MenuData;

namespace LdtPlus.Menu;
public class Menu
{
    public Menu(Gui.Gui gui, MenuRoot menu)
    {
        _gui = gui;
        _input = new();
        _menuPosition = new(menu);
    }

    private readonly Gui.Gui _gui;
    private readonly InputHandler _input;
    private readonly MenuPosition _menuPosition;
    private Command? _result;

    public string GetPath()
    {
        throw new NotImplementedException();
    }

    public Command GetCommand()
    {
        ShowMenu();

        while (_result == null)
        {
            _input.WaitForInput(OnSelect, OnExit, OnMove, OnChar, OnBackspace);
        }

        return _result.Value;
    }

    #region Input handlers
    private void OnSelect()
    {
        if (TryHandleNavigation())
            return;

        _menuPosition.EnterSelected();
        ShowMenu();
    }

    private void OnExit()
    {
        if (!_menuPosition.TryExit())
            SetResult(Command.Exit);

        ShowMenu();
    }

    private void OnMove(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                _menuPosition.ActiveSelection.MoveUp();
                break;
            case ConsoleKey.DownArrow:
                _menuPosition.ActiveSelection.MoveDown();
                break;
            case ConsoleKey.LeftArrow:
                _menuPosition.ActiveSelection.MoveLeft();
                break;
            case ConsoleKey.RightArrow:
                _menuPosition.ActiveSelection.MoveRight();
                break;
        }

        ShowMenu();
    }

    private void OnChar(char c)
    {
        _menuPosition.FilterAdd(c);

        ShowMenu();
    }

    private void OnBackspace()
    {
        if (_menuPosition.TryFilterRemoveLastChar())
            ShowMenu();
    }
    #endregion

    private bool TryHandleNavigation()
    {
        IMenuItem? selectedItem = _menuPosition.SelectedItem;
        if (selectedItem is null)
            return false;

        if (_menuPosition.SelectedItem is not IMenuNav nav)
            return false;
        
        if (nav.TryNavigate(_menuPosition, SetResult))
        {
            ShowMenu();
            return true;
        }

        return false;
    }

    private void SetResult(Command command)
    {
        _result = command;
    }

    private void ShowMenu()
    {
        _gui.Show(batch => batch
            .ShowCommand($"{string.Join(" ", _menuPosition.Path)} {_menuPosition.Filter}")
            .ShowMenu(_menuPosition));
    }
}