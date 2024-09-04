using System.ComponentModel;
using LdtPlus.Menu;

namespace LdtPlus.Gui;
public class GuiMenu
{
    public GuiMenu(ConsoleMain consoleMain, InputHandler inputHandler, MenuRoot menuRoot)
    {
        _inputHandler = inputHandler;
        _menuRoot = menuRoot;

        _consoleOverride = new(consoleMain);
        _components = new();
        _executeCommand = false;
    }

    private readonly InputHandler _inputHandler;
    private readonly MenuRoot _menuRoot;
    private readonly ConsoleOverride _consoleOverride;
    private Stack<MenuComponent> _components;
    private bool _executeCommand;

    public string ShowAndGetCommand()
    {
        Show();

        while (!_executeCommand)
        {
            _inputHandler.WaitForInput();
        }

        string command = ConstructCommand();
        return command;
    }

    public void Show()
    {
        EnterMenu(_menuRoot);
    }

    public void Refresh()
    {
        _consoleOverride.Refresh();
    }

    public void EnterMenu(IMenuContainer menuContainer)
    {
        _components.TryPeek(out MenuComponent? previousMenu);

        // create and add
        MenuComponent menuComponent = new(menuContainer, this);
        menuComponent.UpdateMainFrame();
        _components.Push(menuComponent);
        _consoleOverride.Push(menuComponent.MainFrame);
        _consoleOverride.Refresh();

        // input registration
        if (previousMenu is not null)
            _inputHandler.Unregister(previousMenu);
        _inputHandler.Register(menuComponent);
    }

    public void ExitMenu(out bool menuExited)
    {
        // already no menu - shouldn't happen
        if (!_components.TryPop(out MenuComponent? previousMenu))
        {
            menuExited = false;
            return;
        }

        // there is no upper menu
        if (!_components.TryPeek(out MenuComponent? currentMenu) || _consoleOverride.TryPop())
        {
            menuExited = false;
            return;
        }

        // input registration
        _inputHandler.Unregister(previousMenu);
        _inputHandler.Register(currentMenu);
        menuExited = true;
    }

    public void UpdateShowedCommand(string newValue)
    {
        #warning TODO
    }

    public void ExecuteCommand()
    {
        _executeCommand = true;
    }

    private string ConstructCommand()
    {
        return string.Join(" ", _components.Select(c => c.ActiveKey));
    }
}