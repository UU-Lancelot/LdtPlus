
using LdtPlus.Exceptions;
using LdtPlus.Gui.Interfaces;
using LdtPlus.Gui.Tools;
using LdtPlus.Menu;

namespace LdtPlus.Gui;
public class MainGui : IDisposable, IAsyncDisposable, IInputReceiver
{
    public MainGui(InputHandler inputHandler)
    {
        _console = MainComponent.StartNew();
        _inputHandler = inputHandler;

        _inputHandler.Register(this);
    }

    private readonly MainComponent _console;
    private readonly InputHandler _inputHandler;

    public void ShowHeader()
    {
        HeaderComponent header = new(_console);
        header.Rerender();
    }

    public IDisposable ShowLoader()
    {
        return LoaderComponent.ShowNew(_console);
    }

    public void UseMenu(MenuRoot menuStructure, out string command)
    {
        MenuPosition menuPosition = new(menuStructure);
        CommandComponent commandComponent = new(_console, menuPosition);
        commandComponent.Rerender();

        MenuGui menu = new(_console, menuPosition);
        menu.Show();

        _inputHandler.WaitForInput();
    }

    public void UsePathInput(out string path)
    {
        throw new NotImplementedException();
    }

    public void ShowError(Exception ex)
    {
        ErrorComponent error = new(_console, ex.Message);
        error.Rerender();
    }

    public void Dispose()
    {
        _console.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _console.DisposeAsync();
    }

    public void HandleInput(ConsoleKeyInfo keyInfo, out bool passToNextReceiver)
    {
        if (keyInfo.Key == ConsoleKey.Escape)
            throw new ExitAppException();

        passToNextReceiver = true;
    }
}