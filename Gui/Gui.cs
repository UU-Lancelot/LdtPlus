using LdtPlus.Gui.Tools;

namespace LdtPlus.Gui;
public class Gui : IDisposable, IAsyncDisposable
{
    public Gui()
    {
        _guiBase = new GuiBase();
        _showType = ShowType.Override;

        _guiBase.Start();
        _guiBase.Add(new TitleComponent(_guiBase));
    }

    private GuiBase _guiBase;
    private ShowType _showType;

    public IDisposable ShowLoader()
    {
        LoaderComponent loader = new(_guiBase);
        _guiBase.Add(loader);

        return loader;
    }

    public Gui ShowCommand(string command)
    {
        return this;
    }

    public Gui ShowPath()
    {
        return this;
    }

    public Gui ShowMenu(MenuPosition menu)
    {
        MenuComponent component = new(_guiBase, menu);
        component.Rerender();

        return this;
    }

    public Gui ShowPathInput()
    {
        return this;
    }

    public Gui With()
    {
        _showType = ShowType.Append;

        return this;
    }

    public void Dispose()
    {
        _guiBase.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _guiBase.DisposeAsync();
    }

    private enum ShowType
    {
        Override,
        Append,
    }
}