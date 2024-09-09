using Spectre.Console;

namespace LdtPlus.Gui;
public class CommandComponent
{
    public CommandComponent(ConsoleMain consoleMain)
    {
        _consoleMain = consoleMain;
        _consoleKey = Guid.NewGuid().ToString();
        _components = new();
        _filter = string.Empty;
        MainFrame = new(FullCommand);

        _consoleMain.Add(_consoleKey, MainFrame);
    }

    private readonly ConsoleMain _consoleMain;
    private readonly string _consoleKey;
    private Stack<MenuComponent> _components;
    private string _filter;

    public Text MainFrame { get; private set; }
    public string FullCommand => string.Join(" ", _components.Select(c => c.ActiveKey).Append(_filter).Prepend("ldt"));

    public void PushMenu(MenuComponent menuComponent)
    {
        _components.Push(menuComponent);
        Refresh();
    }

    public void PopMenu()
    {
        _components.Pop();
        Refresh();
    }

    public void UpdateFilter(string filter)
    {
        _filter = filter;
        Refresh();
    }

    private void Refresh()
    {
        MainFrame = new Text(FullCommand);
        _consoleMain.Update(_consoleKey, MainFrame);
    }
}
