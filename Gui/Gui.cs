
using LdtPlus.Exceptions;
using LdtPlus.Menu;
using Spectre.Console;

namespace LdtPlus.Gui;
public class Gui : IDisposable, IAsyncDisposable, IInputReceiver
{
    public Gui(InputHandler inputHandler)
    {
        _console = ConsoleMain.StartNew();
        _inputHandler = inputHandler;

        _inputHandler.Register(this);
    }

    private readonly ConsoleMain _console;
    private readonly InputHandler _inputHandler;

    public void ShowHeader()
    {
        _console.Add("header", new Rule("LancelotDeploymentTool+"));
    }

    public LoaderComponent ShowLoader()
    {
        return LoaderComponent.ShowNew(_console);
    }

    public void UseMenu(MenuRoot menuStructure, out string command)
    {
        GuiMenu menu = new(_console, _inputHandler, menuStructure);
        command = menu.ShowAndGetCommand();
    }

    public void UsePathInput(out string path)
    {
        throw new NotImplementedException();
    }

    public void ShowError(Exception ex)
    {
        _console.Add("error title", new Rule("Error"));
        _console.Add("error message", new Text(ex.Message));
        _console.Show();
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