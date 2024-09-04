using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
public class ConsoleOverride
{
    public ConsoleOverride(ConsoleMain main)
    {
        _main = main;
        _consoleKey = Guid.NewGuid().ToString();
        _stack = new();
    }

    private readonly ConsoleMain _main;
    private readonly string _consoleKey;
    private readonly Stack<IRenderable> _stack;

    public void Push(IRenderable renderable)
    {
        _stack.Push(renderable);

        if (_stack.Count() == 1)
            _main.Add(_consoleKey, renderable);
        else
            _main.Update(_consoleKey, renderable);

        _main.Show();
    }

    public bool TryPop()
    {
        if (_stack.Count() <= 1)
        {
            _main.Remove(_consoleKey);
            return false;
        }

        _stack.Pop();
        _main.Update(_consoleKey, _stack.Peek());
        _main.Show();
        return true;
    }

    public void Refresh()
    {
        if (!_stack.TryPeek(out IRenderable? renderable))
            return;

        _main.Update(_consoleKey, renderable);
        _main.Show();
    }
}
