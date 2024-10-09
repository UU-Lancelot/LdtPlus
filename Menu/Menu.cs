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
    public Menu(Gui.Gui gui, MenuPosition menuPosition)
    {
        _gui = gui;
        _input = new();
        _menuPosition = menuPosition;
    }

    protected readonly Gui.Gui _gui;
    protected readonly InputHandler _input;
    protected MenuPosition _menuPosition;
    protected (Command command, string? parameter)? _result;

    public Command GetCommand(out string? parameter)
    {
        // show
        ShowMenu();

        // wait for result
        while (_result is null)
        {
            _input.WaitForInput(OnSelect, OnExit, OnMove, OnChar, OnBackspace);
        }

        // get result
        parameter = _result.Value.parameter;
        var resultCommand = _result.Value.command;

        // reset
        _result = null;

        // return
        return resultCommand;
    }

    public void RefreshMenu(MenuRoot menu)
    {
        _menuPosition = new(menu);
    }

    #region Input handlers
    private void OnSelect()
    {
        if (_menuPosition.SelectedItem is null)
            return;

        _menuPosition.SelectedItem.OnSelect(_menuPosition, SetResult);
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

    private void SetResult(Command command, string? parameter = null)
    {
        _result = (command, parameter);
    }

    protected virtual void ShowMenu()
    {
        _gui.Show(batch => batch
            .ShowCommand($"{string.Join(" ", _menuPosition.Path.Select(p => p.Split(',')[0]))} {_menuPosition.Filter}")
            .ShowMenu(_menuPosition));
    }
}