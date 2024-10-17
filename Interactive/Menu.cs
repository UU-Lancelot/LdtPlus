using LdtPlus.Interactive.MenuData;
using LdtPlus.Interactive.MenuResults;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive;
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
    protected Result? _result;

    public Result GetCommand()
    {
        // show
        ShowMenu();

        // wait for result
        while (_result is null)
        {
            _input.WaitForInput(OnSelect, OnExit, OnMove, OnChar, OnBackspace);
        }

        // get result
        Result result = _result;

        // reset
        _result = null;

        // return
        return result;
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

        _menuPosition.SelectedItem.OnSelect(_gui, _menuPosition, SetResult);
        ShowMenu();
    }

    private void OnExit()
    {
        if (!_menuPosition.TryExit())
            SetResult(new ResultQuit());

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

    private void SetResult(Result result)
    {
        _result = result;
    }

    protected virtual void ShowMenu()
    {
        _gui.Show(batch => batch
            .ShowText($"Command: ldt {_menuPosition.GenerateCommand()}")
            .ShowMenu(_menuPosition));
    }
}