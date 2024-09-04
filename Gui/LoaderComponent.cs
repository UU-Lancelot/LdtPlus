using Spectre.Console;

namespace LdtPlus.Gui;
public class LoaderComponent : IDisposable
{
    internal LoaderComponent(ConsoleMain visible)
    {
        _visible = visible;
        _key = Guid.NewGuid().ToString();
    }

    private readonly ConsoleMain _visible;
    private readonly string _key;

    internal void Show()
    {
        _visible.Add(_key, new Text("Loading..."));
        _visible.Show();
    }

    public void Dispose()
    {
        _visible.Remove(_key);
        _visible.Show();
    }

    internal static LoaderComponent ShowNew(ConsoleMain visible)
    {
        LoaderComponent loader = new(visible);
        loader.Show();

        return loader;
    }
}